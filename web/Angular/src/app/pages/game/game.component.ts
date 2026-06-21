import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { takeWhile } from 'rxjs/operators';
import { HubService } from '../../services/hub.service';
import { AuthService } from '../../services/auth.service';
import { GameQuestionDto, AnswerOptionDto, PlayerScoreDto } from '../../models/game.models';

export type AnswerState = 'available' | 'selected' | 'correct' | 'wrong';

export interface AnswerButton { answer: AnswerOptionDto; state: AnswerState; }

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrl: './game.component.css'
})
export class GameComponent implements OnInit, OnDestroy {
  roomCode = '';
  question: GameQuestionDto | null = null;
  answerButtons: AnswerButton[] = [];
  timeLeft = 0;
  timeLimitSeconds = 0;
  myScore = 0;
  pointsEarned = 0;
  showPoints = false;
  status = '';
  roundScores: PlayerScoreDto[] = [];
  showRoundResults = false;
  inputEnabled = true;
  private selectedAnswerId = -1;
  private subs: Subscription[] = [];
  private timerSub: Subscription | null = null;

  get timerPercent(): number {
    return this.timeLimitSeconds > 0 ? this.timeLeft / this.timeLimitSeconds : 0;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hub: HubService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.roomCode = this.route.snapshot.paramMap.get('code') ?? '';

    const stored = sessionStorage.getItem('currentQuestion');
    if (stored) {
      this.loadQuestion(JSON.parse(stored) as GameQuestionDto);
      sessionStorage.removeItem('currentQuestion');
    }

    this.subs.push(
      this.hub.questionStarted$.subscribe(q => this.loadQuestion(q)),
      this.hub.answerReceived$.subscribe(r => {
        this.pointsEarned = r.pointsEarned;
        this.showPoints = r.pointsEarned > 0;
        this.status = r.message ?? (r.success ? 'Correct!' : 'Wrong!');
      }),
      this.hub.playerAnswered$.subscribe(name => {
        if (name !== this.auth.currentUser?.username) {
          this.status = `${name} answered!`;
        }
      }),
      this.hub.questionEnded$.subscribe(r => {
        this.timerSub?.unsubscribe();
        this.inputEnabled = false;
        this.answerButtons.forEach(btn => {
          if (btn.answer.answerId === r.correctAnswerId) {
            btn.state = 'correct';
          } else if (btn.answer.answerId === this.selectedAnswerId) {
            btn.state = 'wrong';
          }
        });
        this.roundScores = r.playerScores;
        const me = r.playerScores.find(p => p.userId === this.auth.currentUser?.userId);
        if (me) this.myScore = me.totalScore;
        this.showRoundResults = true;
      }),
      this.hub.gameEnded$.subscribe(r => {
        sessionStorage.setItem('gameResult', JSON.stringify(r));
        this.router.navigate(['/results']);
      }),
      this.hub.error$.subscribe(msg => this.status = msg)
    );
  }

  private loadQuestion(q: GameQuestionDto): void {
    this.question = q;
    this.selectedAnswerId = -1;
    this.showRoundResults = false;
    this.showPoints = false;
    this.status = '';
    this.inputEnabled = true;
    this.answerButtons = q.answers.map(a => ({ answer: a, state: 'available' as AnswerState }));
    this.timeLimitSeconds = q.timeLimitSeconds;
    this.timeLeft = q.timeLimitSeconds;
    this.startTimer();
  }

  private startTimer(): void {
    this.timerSub?.unsubscribe();
    this.timerSub = interval(1000).pipe(
      takeWhile(() => this.timeLeft > 0)
    ).subscribe(() => {
      this.timeLeft--;
      if (this.timeLeft === 0) this.inputEnabled = false;
    });
  }

  async selectAnswer(btn: AnswerButton): Promise<void> {
    if (!this.inputEnabled || this.selectedAnswerId !== -1) return;
    this.selectedAnswerId = btn.answer.answerId;
    btn.state = 'selected';
    this.inputEnabled = false;
    this.timerSub?.unsubscribe();
    try {
      await this.hub.submitAnswer(this.roomCode, this.question!.questionId, btn.answer.answerId);
    } catch (e: unknown) {
      this.status = e instanceof Error ? e.message : 'Error submitting answer';
    }
  }

  answerClass(btn: AnswerButton): string {
    return `answer-btn answer-btn--${btn.state}`;
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
    this.timerSub?.unsubscribe();
  }
}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HubService } from '../../services/hub.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { PlayerDto } from '../../models/room.models';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrl: './room.component.css'
})
export class RoomComponent implements OnInit, OnDestroy {
  roomCode = '';
  quizTitle = '';
  players: PlayerDto[] = [];
  isHost = false;
  status = '';
  busy = false;
  countdown = 0;
  countingDown = false;
  private subs: Subscription[] = [];
  private countdownTimer: ReturnType<typeof setInterval> | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hub: HubService,
    private api: ApiService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.roomCode = this.route.snapshot.paramMap.get('code') ?? '';
    this.isHost = sessionStorage.getItem('isHost') === 'true';

    this.api.getRoom(this.roomCode).subscribe({
      next: state => {
        this.quizTitle = state.quizTitle ?? '';
        this.players = state.players;
      },
      error: () => { /* may not be loaded yet */ }
    });

    this.subs.push(
      this.hub.roomState$.subscribe(s => {
        this.players = s.players;
        this.quizTitle = s.quizTitle ?? '';
      }),
      this.hub.playerJoined$.subscribe(p => {
        if (!this.players.find(x => x.userId === p.userId)) {
          this.players = [...this.players, p];
        }
      }),
      this.hub.playerLeft$.subscribe(name => {
        this.players = this.players.filter(p => p.username !== name);
      }),
      this.hub.gameStarting$.subscribe(cd => this.startCountdown(cd)),
      this.hub.questionStarted$.subscribe(q => {
        sessionStorage.setItem('currentQuestion', JSON.stringify(q));
        this.router.navigate(['/game', this.roomCode]);
      }),
      this.hub.error$.subscribe(msg => this.status = msg)
    );
  }

  private startCountdown(seconds: number): void {
    this.countdown = seconds;
    this.countingDown = true;
    this.countdownTimer = setInterval(() => {
      this.countdown--;
      if (this.countdown <= 0) {
        clearInterval(this.countdownTimer!);
        this.countingDown = false;
      }
    }, 1000);
  }

  async startGame(): Promise<void> {
    this.busy = true;
    try {
      await this.hub.startGame(this.roomCode);
    } catch (e: unknown) {
      this.status = e instanceof Error ? e.message : 'Error starting game';
      this.busy = false;
    }
  }

  async leave(): Promise<void> {
    try { await this.hub.leaveRoom(this.roomCode); } catch { /* ignore */ }
    this.router.navigate(['/lobby']);
  }

  get currentUserId(): number {
    return this.auth.currentUser?.userId ?? -1;
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
    if (this.countdownTimer) clearInterval(this.countdownTimer);
  }
}

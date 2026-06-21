import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { HubService } from '../../services/hub.service';
import { QuizSummaryDto } from '../../models/quiz.models';
import { RoomDto } from '../../models/room.models';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.css'
})
export class LobbyComponent implements OnInit, OnDestroy {
  quizzes: QuizSummaryDto[] = [];
  rooms: RoomDto[] = [];
  selectedQuiz: QuizSummaryDto | null = null;
  joinCode = '';
  status = '';
  busy = false;
  private subs: Subscription[] = [];

  get welcome(): string {
    return `Welcome, ${this.auth.currentUser?.username ?? ''}!`;
  }

  constructor(
    private auth: AuthService,
    private api: ApiService,
    private hub: HubService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.api.getQuizzes().subscribe({
      next: q => this.quizzes = q,
      error: () => { /* backend may not be running */ }
    });
    this.api.getActiveRooms().subscribe({
      next: r => this.rooms = r,
      error: () => { /* backend may not be running */ }
    });
  }

  selectQuiz(q: QuizSummaryDto): void {
    this.selectedQuiz = this.selectedQuiz === q ? null : q;
  }

  async createRoom(): Promise<void> {
    if (!this.selectedQuiz) return;
    this.busy = true;
    this.status = '';
    try {
      await this.hub.connect();
      this.api.createRoom({ quizId: this.selectedQuiz.quizId, maxPlayers: 8 }).subscribe({
        next: async (room) => {
          await this.hub.joinRoom(room.code);
          sessionStorage.setItem('roomCode', room.code);
          sessionStorage.setItem('isHost', 'true');
          this.router.navigate(['/room', room.code]);
        },
        error: (e: { error?: { error?: string }; message?: string }) => {
          this.status = e.error?.error ?? 'Error creating room';
          this.busy = false;
        }
      });
    } catch (e: unknown) {
      this.status = e instanceof Error ? e.message : 'Error connecting';
      this.busy = false;
    }
  }

  async joinRoom(code?: string): Promise<void> {
    const roomCode = (code ?? this.joinCode).trim().toUpperCase();
    if (!roomCode) return;
    this.busy = true;
    this.status = '';
    try {
      await this.hub.connect();
      await this.hub.joinRoom(roomCode);
      sessionStorage.setItem('roomCode', roomCode);
      sessionStorage.setItem('isHost', 'false');
      this.router.navigate(['/room', roomCode]);
    } catch (e: unknown) {
      this.status = e instanceof Error ? e.message : 'Error joining room';
      this.busy = false;
    }
  }

  logout(): void {
    this.auth.logout();
    this.hub.disconnect();
    this.router.navigate(['/login']);
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }
}

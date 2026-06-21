import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HubService } from '../../services/hub.service';
import { GameResultDto, LeaderboardEntryDto } from '../../models/game.models';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrl: './results.component.css'
})
export class ResultsComponent implements OnInit {
  leaderboard: LeaderboardEntryDto[] = [];
  myEntry: LeaderboardEntryDto | null = null;
  winner = '';

  constructor(
    private auth: AuthService,
    private hub: HubService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const raw = sessionStorage.getItem('gameResult');
    if (raw) {
      const result: GameResultDto = JSON.parse(raw) as GameResultDto;
      sessionStorage.removeItem('gameResult');
      this.leaderboard = result.leaderboard ?? [];
      this.winner = this.leaderboard[0]?.username ?? '';
      this.myEntry = this.leaderboard.find(e => e.userId === this.auth.currentUser?.userId) ?? null;
    }
  }

  medalFor(rank: number): string {
    if (rank === 1) return '🥇';
    if (rank === 2) return '🥈';
    if (rank === 3) return '🥉';
    return '';
  }

  async backToLobby(): Promise<void> {
    await this.hub.disconnect();
    this.router.navigate(['/lobby']);
  }
}

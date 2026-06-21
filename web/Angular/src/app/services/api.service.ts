import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { QuizSummaryDto } from '../models/quiz.models';
import { CreateRoomRequest, RoomDto, RoomStateDto } from '../models/room.models';
import { LeaderboardEntryDto } from '../models/game.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  getQuizzes() { return this.http.get<QuizSummaryDto[]>('/api/quizzes'); }
  getActiveRooms() { return this.http.get<RoomDto[]>('/api/rooms/active'); }
  getRoom(code: string) { return this.http.get<RoomStateDto>(`/api/rooms/${code}`); }
  createRoom(req: CreateRoomRequest) { return this.http.post<RoomStateDto>('/api/rooms', req); }
  getLeaderboard() { return this.http.get<LeaderboardEntryDto[]>('/api/leaderboard'); }
}

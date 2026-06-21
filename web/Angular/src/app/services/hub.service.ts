import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { AuthService } from './auth.service';
import { RoomStateDto, PlayerDto } from '../models/room.models';
import { GameQuestionDto, QuestionResultDto, GameResultDto, AnswerResultDto } from '../models/game.models';

@Injectable({ providedIn: 'root' })
export class HubService implements OnDestroy {
  private connection: HubConnection | null = null;

  // Server → client event streams
  readonly roomState$       = new Subject<RoomStateDto>();
  readonly playerJoined$    = new Subject<PlayerDto>();
  readonly playerLeft$      = new Subject<string>();
  readonly gameStarting$    = new Subject<number>();
  readonly questionStarted$ = new Subject<GameQuestionDto>();
  readonly answerReceived$  = new Subject<AnswerResultDto>();
  readonly playerAnswered$  = new Subject<string>();
  readonly questionEnded$   = new Subject<QuestionResultDto>();
  readonly gameEnded$       = new Subject<GameResultDto>();
  readonly error$           = new Subject<string>();

  constructor(private auth: AuthService) {}

  get state(): HubConnectionState {
    return this.connection?.state ?? HubConnectionState.Disconnected;
  }

  async connect(): Promise<void> {
    if (this.connection) await this.disconnect();

    this.connection = new HubConnectionBuilder()
      .withUrl('/hubs/quiz', {
        accessTokenFactory: () => this.auth.token ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();
    await this.connection.start();
  }

  private registerHandlers(): void {
    const c = this.connection!;
    c.on('RoomState',    (dto: RoomStateDto) => this.roomState$.next(dto));
    c.on('PlayerJoined', (dto: PlayerDto) => this.playerJoined$.next(dto));
    c.on('PlayerLeft',   (name: string) => this.playerLeft$.next(name));
    c.on('GameStarting', (data: { countdown: number }) => this.gameStarting$.next(data?.countdown ?? 3));
    c.on('QuestionStarted', (dto: GameQuestionDto) => this.questionStarted$.next(dto));
    c.on('AnswerReceived',  (data: AnswerResultDto) => this.answerReceived$.next(data));
    c.on('PlayerAnsweredNotification', (name: string) => this.playerAnswered$.next(name));
    c.on('QuestionEnded', (dto: QuestionResultDto) => this.questionEnded$.next(dto));
    c.on('GameEnded',     (dto: GameResultDto) => this.gameEnded$.next(dto));
    c.on('Error',         (msg: string) => this.error$.next(msg));
  }

  joinRoom(code: string): Promise<void>  { return this.connection!.invoke('JoinRoom', code); }
  leaveRoom(code: string): Promise<void> { return this.connection!.invoke('LeaveRoom', code); }
  startGame(code: string): Promise<void> { return this.connection!.invoke('StartGame', code); }
  submitAnswer(code: string, questionId: number, answerId: number): Promise<void> {
    return this.connection!.invoke('SubmitAnswer', code, questionId, answerId);
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }

  ngOnDestroy(): void { this.disconnect(); }
}

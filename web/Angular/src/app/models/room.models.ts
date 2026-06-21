export type RoomStatus = 'Waiting' | 'Active' | 'Completed';
export interface PlayerDto { userId: number; username: string; isHost: boolean; }
export interface RoomDto { code: string; quizTitle?: string; playerCount: number; status: RoomStatus; maxPlayers: number; }
export interface RoomStateDto extends RoomDto { players: PlayerDto[]; }
export interface CreateRoomRequest { quizId: number; maxPlayers?: number; }

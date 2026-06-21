export interface AnswerOptionDto { answerId: number; text: string; }
export interface GameQuestionDto { questionId: number; text: string; timeLimitSeconds: number; points: number; questionNumber: number; totalQuestions: number; answers: AnswerOptionDto[]; }
export interface PlayerScoreDto { userId: number; username: string; roundPoints: number; totalScore: number; rank: number; }
export interface QuestionResultDto { correctAnswerId: number; playerScores: PlayerScoreDto[]; }
export interface LeaderboardEntryDto { userId: number; username: string; totalScore: number; correctAnswers: number; totalQuestions: number; rank: number; }
export interface GameResultDto { leaderboard: LeaderboardEntryDto[]; }
export interface AnswerResultDto { success: boolean; pointsEarned: number; message?: string; }

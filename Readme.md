# QuizApp API

QuizApp is a web application that allows users to create, manage, and interact with quizzes. This API supports user registration, authentication, quiz creation, and various user roles such as Admin and Member. It also allows users to rate quizzes, submit answers, view leaderboards, and track quiz attempts.

## Features

- **User Authentication & Authorization**: Registration, login, role management (Admin, User, Member).
- **Quiz Management**: Users can create, update, delete quizzes, view, take quizzes and submit answers.
- **Question Management**: Users can add, update, and remove quiz questions.
- **Quiz Rating**: Users can rate quizzes after attempting them.
- **Leaderboard**: Displays top performers based on their quiz attempts.
- **Quiz Attempts History**: Users can view their quiz attempt history.
- **Search Functionality**: Search quizzes by category, difficulty, or name.
- **Reporting & User Management**: Admins can ban/unban users and view reports.

## API Endpoints

### Authentication & User Management

#### POST /registerme
- Registers a new user with the provided email and password.
- Assigns `User` and `Member` roles to the new user.

### Admin Endpoints

#### GET /admin/quizzes/pending
- Fetches all quizzes pending approval.

#### PUT /admin/quizzes/{id}/status
- Updates the status of a quiz (approve, reject, etc.).

#### GET /admin/users
- Retrieves a list of all users.

#### POST /admin/users/{userId}/ban
- Bans a user by their user ID.

#### POST /admin/users/{userId}/unban
- Unbans a user by their user ID.

#### GET /admin/reports
- Retrieves a list of reports.

### Quiz Endpoints

#### POST /quiz
- Creates a new quiz.

#### PUT /quiz/{id}
- Updates an existing quiz.

#### DELETE /quiz/{id}
- Deletes a quiz.

#### GET /quiz/GetByCategory/{CategoryType}
- Fetches quizzes by category type.

#### GET /quiz/GetByDifficulty/{DifficultyType}
- Fetches quizzes by difficulty type.

#### POST /quiz/{quizId}/rate
- Rates a quiz by its ID.

#### GET /quiz/leaderboard
- Retrieves the leaderboard based on performance.

### Quiz Interaction Endpoints

#### GET /quiz-interaction/search
- Searches for quizzes by name, category, or difficulty.

#### POST /quiz-interaction/start
- Starts a quiz session for a user.

#### POST /quiz-interaction/check-answer
- Checks the correctness of an answer submission.

#### POST /quiz-interaction/submit-attempt
- Submits a quiz attempt after completing it.

#### GET /quiz-interaction/attempt-history
- Retrieves the history of quiz attempts for a user.

## Setup & Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Mostafa-khatab/QuizApp.git
   cd QuizApp

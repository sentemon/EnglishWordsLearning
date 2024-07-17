# EnglishWordsLearning

## Description
EnglishWordsLearning is a web application for learning English words using flashcards and tests.

## Requirements
- .NET SDK 9.0
- Docker
- Docker Compose

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/sentemon/EnglishWordsLearning.git
   cd EnglishWordsLearning/EnglishWordsLearning.Web
   ```

2. 
    ```bash
    docker compose up -d --build
    ```

3. 
    ```bash
    dotnet ef database update
    ```

4. 
    ```bash
    dotnet run
    ```
# EnglishWordsLearning

## Description
EnglishWordsLearning is a web application designed to help users learn English words through the use of flashcards and tests. **Currently, words can only be learned from Russian to English.**

## Requirements
- .NET SDK 9.0
- Docker
- Docker Compose

## Installation

Follow these steps to install and run the application:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/sentemon/EnglishWordsLearning.git
   cd EnglishWordsLearning/EnglishWordsLearning.Web
   ```

2. **Build and start the Docker containers:**
   ```bash
   docker compose up -d --build
   ```

3. **Update the database:**
   ```bash
   dotnet ef database update
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```
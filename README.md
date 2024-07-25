# EnglishWordsLearning

## Description
EnglishWordsLearning is a web application designed to help users learn English words through the use of flashcards and tests. **Currently, words can only be learned from Russian to English.**

## Requirements
- Docker
- Docker Compose

## Installation

Follow these steps to install and run the application:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/sentemon/EnglishWordsLearning.git
   cd EnglishWordsLearning
   ```

2. **Build and start the Docker containers:**
   ```bash
   docker compose up -d --build
   ```

3. **Accessing the Application**
   ```
   You can access the application via your web browser at http://localhost:8080
   ```
4. **Stopping the Application**
   ```bash
   docker compose down
   ```
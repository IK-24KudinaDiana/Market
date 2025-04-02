pipeline {
    agent any

    environment {
        DOTNET_VERSION = '8.0'
        MAIN_PROJECT = 'WebApi/WebApi.csproj'  // Шлях до основного проекту в репозиторії
        TEST_PROJECT = 'TradeMarket.Tests/TradeMarket.Tests.csproj'  // Шлях до тестового проекту в репозиторії
    }

    stages {
        
        stage('Setup .NET SDK') {
            steps {
                script {
                    // Перевірка, чи встановлений .NET SDK на Jenkins
                    def dotnetInstalled = sh(script: 'dotnet --version', returnStatus: true) == 0
                    if (!dotnetInstalled) {
                        error 'SDK .NET не встановлений. Переконайтесь, що Jenkins має доступ до SDK .NET 8.'
                    }
                }
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Відновлення залежностей для всіх проектів (основний проект і тестовий)
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                // Побудова основного проекту (WebApi)
                sh "dotnet build ${MAIN_PROJECT} --configuration Release --no-restore"
            }
        }

        stage('Run Unit Tests') {
            steps {
                // Запуск тестів для тестового проекту
                sh 'dotnet test TradeMarket.Tests/TradeMarket.Tests.csproj --configuration Release --no-build --logger trx'
            }
        }

        stage('Publish') {
            steps {
                // Публікація основного проекту WebApi
                sh "dotnet publish ${MAIN_PROJECT} --configuration Release --output publish"
            }
        }

        stage('Archive Build') {
            steps {
                // Архівація артефактів
                archiveArtifacts artifacts: 'publish/**', fingerprint: true
            }
        }

        stage('Deploy') {
            steps {
                echo 'Deploy stage: Налаштуйте необхідний деплоймент сервіс (наприклад, Docker або Kubernetes).'
            }
        }
    }

    post {
        success {
            echo '✅ Побудова успішна!'
        }
        failure {
            echo '❌ Помилка під час виконання конвеєра.'
        }
    }
}
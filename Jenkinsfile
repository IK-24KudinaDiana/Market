pipeline {
    agent any

    environment {
        DOTNET_8 = '/usr/share/dotnet/sdk/8.0'  // Шлях до SDK 8.0
        DOTNET_6 = '/usr/share/dotnet/sdk/6.0'  // Шлях до SDK 6.0
        MAIN_PROJECT = 'WebApi/WebApi.csproj'  // Шлях до основного проекту в репозиторії
        TEST_PROJECT = 'TradeMarket.Tests/TradeMarket.Tests.csproj'  // Шлях до тестового проекту в репозиторії
    }

    stages {

        stage('Setup .NET SDK') {
            steps {
                script {
                    // Перевірка, чи встановлені потрібні версії .NET SDK
                    def dotnet6Installed = sh(script: 'dotnet --list-sdks | grep "6.0"', returnStatus: true) == 0
                    def dotnet8Installed = sh(script: 'dotnet --list-sdks | grep "8.0"', returnStatus: true) == 0
                    
                    if (!dotnet6Installed || !dotnet8Installed) {
                        error 'Необхідні версії .NET SDK не встановлені. Переконайтесь, що Jenkins має доступ до SDK .NET 6 і 8.'
                    }
                }
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Відновлення залежностей для всіх проектів
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                // Побудова основного проекту (WebApi) за допомогою .NET 8
                sh "dotnet build ${MAIN_PROJECT} --configuration Release --no-restore"
            }
        }

        stage('Run Unit Tests') {
            steps {
                script {
                    // Використовуємо .NET 6 для запуску тестів
                    sh "export DOTNET_ROOT=${env.DOTNET_6} && dotnet test ${TEST_PROJECT} --configuration Release --no-build --logger trx"
                }
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
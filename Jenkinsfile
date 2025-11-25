pipeline {
    agent any

    environment {
        APP_NAME = "justixapi"
        REGISTRY = "docker.io/mensahelikem44850"
        IMAGE = "${REGISTRY}/${APP_NAME}"
    }

    stages {

        stage('Build Docker Image') {
            steps {
                sh "docker build -t ${IMAGE}:${BUILD_NUMBER} -t ${IMAGE}:latest ."
            }
        }

        stage('Push Docker Image') {
            steps {
                withCredentials([usernamePassword(
                    credentialsId: 'docker-creds',
                    usernameVariable: 'USER',
                    passwordVariable: 'PASS'
                )]) {
                    sh """
                        echo "$PASS" | docker login -u "$USER" --password-stdin
                        docker push ${IMAGE}:${BUILD_NUMBER}
                        docker push ${IMAGE}:latest
                    """
                }
            }
        }

        stage('Deploy to K8s') {
            steps {
                withCredentials([file(credentialsId: 'Kubeconfig-local', variable: 'KCFG')]) {
                    sh """
                        mkdir -p ~/.kube
                        cp "$KCFG" ~/.kube/config
                        kubectl rollout restart deployment ${APP_NAME} -n dev
                    """
                }
            }
        }
    }

    post {
        always {
            sh 'docker logout'
        }
    }
}

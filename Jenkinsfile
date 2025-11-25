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
                withCredentials([usernamePassword(credentialsId: 'docker-hub-creds',
                                                  usernameVariable: 'DOCKER_USER',
                                                  passwordVariable: 'DOCKER_PASS')]) {
                    sh """
                        echo "$DOCKER_PASS" | docker login -u "$DOCKER_USER" --password-stdin
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

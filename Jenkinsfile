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
                withCredentials([usernamePassword(credentialsId: 'docker-creds',
                                                  usernameVariable: 'DOCKER_USER',
                                                  passwordVariable: 'DOCKER_PASS')]) {
                    sh """
                        echo "$DOCKER_PASS" | docker login -u "$DOCKER_USER" --password-stdin
                    """
                }
            }
        }

        stage('Apply Deployment Manifests') {
            steps {
                withCredentials([file(credentialsId: 'Kubeconfig-local', variable: 'KCFG')]) {
                    sh """
                        mkdir -p ~/.kube
                        cp "$KCFG" ~/.kube/config

                        # Apply deployment and service manifests
                        kubectl apply -f k8s/deployment.yaml -n dev
                    """
                }
            }
        }

        stage('Restart Deployment') {
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

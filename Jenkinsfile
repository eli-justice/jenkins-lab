pipeline {
    agent any

    environment {
        APP_NAME = "justixapi"
        REGISTRY = "docker.io/mensahelikem44850"
        IMAGE = "${REGISTRY}/${APP_NAME}"
        KUBECONFIG_CONTENT = credentials('Kubeconfig-local')
        DOCKER_USER = credentials('docker-username')
        DOCKER_PASS = credentials('docker-password')
    }

    stages {
        stage('Build Docker Image') {
            steps {
                script {
                    sh "docker build -t ${IMAGE}:${BUILD_NUMBER} -t ${IMAGE}:latest ."
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                script {
                    sh """
                        echo ${DOCKER_PASS} | docker login -u ${DOCKER_USER} --password-stdin
                        docker push ${IMAGE}:${BUILD_NUMBER}
                        docker push ${IMAGE}:latest
                    """
                }
            }
        }

        stage('Deploy to K8s') {
            steps {
                script {
                    sh """
                        mkdir -p ~/.kube
                        echo '${KUBECONFIG_CONTENT}' > ~/.kube/config
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
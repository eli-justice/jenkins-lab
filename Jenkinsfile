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

        stage('Checkout') {
            steps {
                git branch: 'dev', url: 'https://github.com/eli-justice/jenkins-lab.git'
            }
        }

        stage('Docker Login') {
            steps {
                sh '''
                  echo "$DOCKER_PASS" | docker login -u "$DOCKER_USER" --password-stdin
                '''
            }
        }

        stage('Build Image') {
            steps {
              dir(src/Egapay.Customer.Business.Gateway.API) {
                sh '''
                  docker build -t docker.io/mensahelikem44850/justixapi:${BUILD_NUMBER} ..
                '''
            }
        }
     }

        stage('Push Image') {
            steps {
                sh '''
                  docker push ${IMAGE}:${BUILD_NUMBER}
                '''
            }
        }

        stage('Deploy to K8s') {
            steps {
                sh '''
                  mkdir -p $WORKSPACE/kube
                  echo "$KUBECONFIG_CONTENT" > $WORKSPACE/kube/config
                  export KUBECONFIG=$WORKSPACE/kube/config

                  kubectl set image deployment/${APP_NAME} \
                    ${APP_NAME}=${IMAGE}:${BUILD_NUMBER}
                '''
            }
        }

        stage('Rollout Restart') {
            steps {
                sh '''
                  export KUBECONFIG=$WORKSPACE/kube/config
                  kubectl rollout restart deployment/${APP_NAME}
                '''
            }
        }

        stage('Verify') {
            steps {
                sh '''
                  export KUBECONFIG=$WORKSPACE/kube/config
                  kubectl rollout status deployment/${APP_NAME}
                '''
            }
        }
    }
}

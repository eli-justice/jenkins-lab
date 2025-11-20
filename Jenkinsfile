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
           stage('Build Docker Image') {
               steps {
                   script {
                       sh 'docker build -t docker.io/mensahelikem44850/justixapi:latest .'
                   }
               }
           }

           stage('Push Docker Image') {
               steps {
                   script {
                       sh 'docker push docker.io/mensahelikem44850/justixapi:latest'
                   }
               }
           }

           stage('Deploy to K8s') {
               steps {
                   script {
                       sh 'kubectl rollout restart deployment justixapi -n dev'
                   }
               }
           }
       }
   }


# DotNetCoreWebAPIInGCP

This project is a sample REST API written in .NET Core 3.1 that can be deployed to a Kubernetes (K8s) cluster. All it contains
is a simple healthcheck endpoint and some Swagger API documentation. I plan to build off of this project and explore other
products and tools within GCP. You may find this useful if you want to see how to deploy a .NET Core 3.* Web API application
via K8s!


## Project Dependencies

In order to run and deploy this project, make sure you have the following dependencies installed on your machine:

* .NET Core 3.1 => https://dotnet.microsoft.com/download/dotnet-core/3.1
* Google Cloud SDK => https://cloud.google.com/sdk
* Kubectl => https://kubernetes.io/docs/tasks/tools/install-kubectl/


## Creating a Project in GCP

Log into your Google account that has access to Google Cloud Platform (GCP) with the following command:

`gcloud auth login`

Next, create a project with:

`gcloud projects create [YOUR-PROJECT-ID]`

Verify that the project was created with:

`gcloud projects describe [YOUR-PROJECT-ID]`


## Building a Container From the Dockerfile

In order to build the container, make sure you're in the root directory of this project, which contains the Dockerfile. Then,
make sure `gcloud` is pointing to the project you want to run in GKE (if you just created the project using `gcloud` then you
should be all set):

`gcloud config set project [YOUR-PROJECT-ID]`

Now, build the container with the following:

`gcloud builds submit --tag gcr.io/[YOUR-PROJECT-ID]/[YOUR-CONTAINER-NAME] .`

Wait for the container to build...

Once the container finishes building, you'll see some information, including the name of the container and a status:

```
Created [https://cloudbuild.googleapis.com/v1/projects/YOUR-PROJECT-ID/builds/xxxxxxx-xxxx-xxx-xxx-xxxxxxxxxxxx].
Logs are permanently available at [https://console.developers.google.com/logs/viewer?resource=build&project=YOUR-PROJECT-ID&filters=text:xxxx-xxx-xxx-xxxxxxxxxxxx]].

ID  CREATE_TIME DURATION  SOURCE                                                     IMAGES                               STATUS
xxxxxxx-xxxx-xxx-xxx-xxxxxxxxxxxx  2017-03-04T00:42:10+00:00  1M32S     gs://YOUR-PROJECT-ID_cloudbuild/source/xxxxxxx.08.tgz  gcr.io/YOUR-PROJECT-ID/YOUR-CONTAINER-NAME  SUCCESS<
```


## Run the App in GKE

Next, you need to create the K8s cluster:

`gcloud container clusters create [YOUR-CLUSTER-NAME]`

You also need to set the compute zone (make sure you specify a zone that makes sense for your app):

`gcloud config set compute/zone us-east1-b`

Now, authorize `kubectl` to access your new K8s cluster:

`gcloud container clusters get-credentials [YOUR-CLUSTER-NAME]`

After following the prompts to authorize `kubectl`, you can create a pod for your containerized app:

`kubectl run [YOUR-POD-NAME] --image=gcr.io/[YOUR-PROJECT-ID]/[YOUR-CONTAINER-NAME]`

To see your pods, run:

`kubectl get pods`

Then, you can create a deployment for your app:

`kubectl create deployment [YOUR-DEPLOYMENT-NAME] --image=gcr.io/[YOUR-PROJECT-ID]/[YOUR-CONTAINER-NAME]`

To check your deployments, run:

`kubectl get deployments`

Now, you can expose your deployment to the public on the given port:

`kubectl expose deployment [YOUR-DEPLOYMENT-NAME] --type="LoadBalancer" --port 8080`

Your service should be spinning up! To see what the external IP is for you to access through a web browser, type:

`kubectl get service`

To access your app running on GKE, open up a web browser and navigate to:

`http://EXTERNAL-IP:8080/swagger` or `http://EXTERNAL-IP:8080/api/healthcheck`

Congrats you're live on GKE!!!

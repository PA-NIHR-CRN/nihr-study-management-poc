# NIHR Study Management API

This project is the API entry point for Study Management. It is an ASP .NET Core application with references to Amazon.Lambda.AspNetCoreServer nuget package, allowing the
application to be deployed as a dotnetcore7 docker lambda function in AWS.

The lambda function is exposed via an API Gateway in AWS.

## Local development
For local development, the API can simply be initialised using standard dotnet run (F5) commands. By default the appsettings.development.json file should configure the runtime
to override jwt token validation i.e. bypassing authentication. This is intended for local development only and should never be used in a production environment.

## Packaging as a Docker image.

This project is configured to package the Lambda function as a Docker image. The default configuration for the project and the Dockerfile is to build 
the .NET project on the host machine and then execute the `docker build` command which copies the .NET build artifacts from the host machine into 
the Docker image. 

The `--docker-host-build-output-dir` switch, which is set in the `aws-lambda-tools-defaults.json`, triggers the 
AWS .NET Lambda tooling to build the .NET project into the directory indicated by `--docker-host-build-output-dir`. The Dockerfile 
has a **COPY** command which copies the value from the directory pointed to by `--docker-host-build-output-dir` to the `/var/task` directory inside of the 
image.

Alternatively the Docker file could be written to use [multi-stage](https://docs.docker.com/develop/develop-images/multistage-build/) builds and 
have the .NET project built inside the container.

## To deploy from Visual Studio (using serverless.template)

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*. Most of the default values (aside from AWS credentials)
should default to sandbox entries. Deployments to stable environments such as DEV and beyond will be handled outside of this solution by infrastructure code.



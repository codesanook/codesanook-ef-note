# codesanook-ef-note

## How to run the project locally
- Fork this project
- Clone the project to your computer. !!! Change **{your-github-username}** to your GitHub username.
  ```sh
  git clone git@github.com:{your-github-username}/codesanook-ef-note.git
  ```
- CD to to the root folder.
  ```sh
  cd codesanook-ef-note
  ```
- Launch Docker containers.
  ```sh
  docker compose down --volumes; docker compose up --build
  ```
- Wait for a while until you see dotnet watch messages, e.g.
  ```sh
  web_1  | info: Microsoft.Hosting.Lifetime[0]
  web_1  |       Now listening on: http://[::]:8000
  ```
- Open a browser and navigate to http://localhost:8000/.
- You will find a simple note app that you can:
  - Add a new notebook which is a group/container of each note.
  - Add a new note.
  - Add a new tag.
  - Update/Delete notebook, note and tag.

## Hot reload
- Edit some C# source code in `src/Codesanook.EFNote` folder.
- Code will be compile automatically.
- Refresh a browser and see what you have changed.

## EF Note in a browser
![ef-note-animated-screenshot.gif](ef-note-animated-screenshot.gif)

## Release compose for testing only
```sh
docker-compose down --volumes; docker-compose -f docker-compose.yml -f docker-compose.release.yml up --build
```

## Production release

### Create a new App Service
- Create Azure App Service with a container
- Use DockerHub registry and `mcr.microsoft.com/dotnet/samples:aspnetapp` image
- Check log in deployment, open a browser and navigate to https://{your-app-service-name}.azurewebsites.net/
- You should find an example ASP.NET Core MVC app

### Set some configurations
- Set these configurations to your app service:
  - `WEBSITE_WEBDEPLOY_USE_SCM`
    - `true`
  - `WEBSITES_PORT`
    - `8000`
  - `CONNECTIONSTRINGS__DEFAULTCONNECTION`
    - `Server={your-server-name}.mysql.database.azure.com; Port=3306; Database={your-database-name}; Uid={your-username}@{your-server-name}; Pwd={your-password}; SslMode=Preferred;CharSet=utf8mb4;`
- More details for Npgsql SSL connection https://www.npgsql.org/doc/security.html#encryption-ssltls

### Create DockerHub repository and get a new token
- Create a public DockerHub repository
- Get DockerHub token from Account Settings > Security > New Access Token

### Create GitHub secret
- Download publish profile from your App Service and use it a value of AZURE_WEBAPP_CONTAINER_PUBLISH_PROFILE secret
- Create these GitHub secrets with their values:
  - AZURE_WEBAPP_CONTAINER_PUBLISH_PROFILE
  - AZURE_WEBAPP_NAME
  - DOCKERHUB_REPOSITORY
  - DOCKERHUB_TOKEN
  - DOCKERHUB_USERNAME

### Trigger GitHub Actions
- Go to GitHub Action tab and enable it
- Create new commit and push the project to the main branch

## Debugging
- CD to `src/Codesanook.EFNote` folder and launch the app with debugging `.NET Core launch (web)`.
- Start only a database container at root level folder.
  ```sh
  docker compose up mysql
  # or
  docker compose up mssql
  ```

## Presentation
- [Link to Google slide presentation](https://docs.google.com/presentation/d/1OkDfotFvxa4PNxIj2VksGwfjXWVOAOURDJ59fUcXzzo/edit)

## In memory creative

```sh
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

## Create empty solution file
```sh
dotnet new sln --name Codesanook.EFNote
```

## Add existing project to a solution file
```sh
dotnet sln add ./Codesanook.EFNote/Codesanook.EFNote.csproj
```

## Work with database migration
- CD to root of the project
  ```sh
  cd src/Codesanook.EFNote
  ```
- Install additional required package.
  ```sh
  dotnet add Microsoft.EntityFrameworkCore.Design
  ```
- Create your first migration file.
  ```sh
  dotnet ef migrations add InitialCreate
  ```
- Apply migrations to a database.
  ```
  dotnet ef database update
  ```
- Remove migration files.
  ```
  dotnet ef migrations remove
  ```
- Update the tool
  ```sh
  dotnet tool update --global dotnet-ef
  ```

## TODO
- [ ] Global exception
- [ ] Deploy to a cheap MySQL server
- [x] Improve code quality
- [x] Use async/await

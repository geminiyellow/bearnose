# bearnose

## dotnet core + angular 2

- client: [angular-seed-advanced](https://github.com/NathanWalker/angular-seed-advanced)
- server: [dotnetcore-angular-seed-advanced](https://github.com/Kaffiend/dotnetcore-angular-seed-advanced)

### Usage

```bash
npm start
```

open [localhost:3000](http://localhost:3000)

### Note

#### client

There is too much change, maintenance will sucks your life.

- add `dotnet.config.ts`
  - `DOTNET_TASKS_DIR`
  - `DOTNET_COMPOSITE_TASKS`
  - `DOTNET_SERVER_DEST`
- modify `config.ts`'s base class to `DotNetConfig`

- add dotnet task, and use it in `dotnet.config.ts`
  - `build.dotnet.assets.dev.ts`
  - `build.dotnet.assets.prod.ts`
  - `build.dotnet.index.dev.ts`
  - `build.dotnet.index.prod.ts`
  - `copy.dotnet.dev.ts`
  - `copy.dotnet.prod.ts`
- modify `gulpfile.ts`
  - use `loadTasks` and `loadCompositeTasks` to merge *dotnet* tasks into gulp tasks

- modify `utils/clean`
  - add param `force`, make you can delete files outside the project root

## docker

```docker
docker-compose run --service-ports --rm --name bearnose web
```

PROJECT_DIR=./src/CleanAuth.Infrastructure
STARTUP_PROJECT=./src/CleanAuth.Api
MIGRATION_NAME?=InitialStructure

migrate-create:
	dotnet ef migrations add $(MIGRATION_NAME) --project $(PROJECT_DIR) --startup-project $(STARTUP_PROJECT)

migrate-update:
	dotnet ef database update --project $(PROJECT_DIR) --startup-project $(STARTUP_PROJECT)

migrate-remove:
	dotnet ef migrations remove --project $(PROJECT_DIR) --startup-project $(STARTUP_PROJECT)

migrate-list:
	dotnet ef migrations list --project $(PROJECT_DIR) --startup-project $(STARTUP_PROJECT)

db-drop:
	dotnet ef database drop --project $(PROJECT_DIR) --startup-project $(STARTUP_PROJECT) --force

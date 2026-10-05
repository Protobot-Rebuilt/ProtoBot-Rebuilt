# Building Protobot

[Local build](#local-build)\
[GitHub Actions build](#github-actions-build)

## Local Build

Still working on the process

## GitHub Actions Build

### Requerments

Before running CI builds, ensure you have:

- A Unity account with an active Pro license (A Student license will also work)
- Valid Unity credentials available for repository secrets

### One-Time Repository Setup

1. Open the repository on GitHub.
2. Go to Settings.
3. Navigate to Secrets and variables, then select Actions.
4. Click New repository secret.
5. Create a secret named UNITY_EMAIL and set its value to your Unity account email.
6. Click Add secret.
7. Repeat steps 4 to 6 for UNITY_PASSWORD using your Unity account password.
8. Go to [Unity License](#unity-license) to set up the license used to build

### Run the Workflow

1. Open the repository on GitHub.
2. Click the Actions tab.
3. In the left sidebar, select ProtoBot Build.
4. Click Run workflow.
5. Leave the branch as main unless you intentionally need another branch.
6. Click the green Run workflow button to start.

The workflow should appear in the runs list within a few seconds.

### Download Build Artifacts

After the workflow completes:

1. Open the completed workflow run.
2. Scroll to the Artifacts section.
3. Download the required artifact:
   - ProtobotRebuilt-Installer: Windows installer package
   - ProtoBotRebuilt-macOS-DMG: macOS disk image

Artifacts download as zip files. Extract them to access the installer binaries.

## Unity License


## Troubleshooting

If a workflow fails:

1. Open the failed run in Actions.
2. Identify which job failed.
3. Expand the failed step and review the logs.

Common failure causes:

- Unity credential secrets are missing, expired, or incorrect
- A recent code or asset change introduced a build regression
- Installer script issues (for example, NSIS packaging errors)
- Temporary GitHub Actions service disruption (check https://www.githubstatus.com)

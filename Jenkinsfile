pipeline {
 agent any

  stages {
    stage('Initialize Unity Project') {
      steps {
        bat 'echo "Unity project Initialized"'
        echo "Trying out if this also works"
      }
    }
    stage('Build Windows') {
      environment {
        BUILD_PROFILE = "WindowsMain"
        UNITY_VERSION = "2020.3.21f1"
      }
      steps {
        bat '"C:\\Program Files\\Unity\\Hub\\Editor\\%UNITY_VERSION%\\Editor\\Unity.exe" -quit -accept-apiupdate -batchmode  -logFile -projectPath "%WORKSPACE%/" -executeMethod GenericBuildCommand.BuildWin64'

        echo "So far so good";
      }
    }
    // stage('Compressing Artifacts') {
    //   steps {
    //     script {
    //       bat 'rmdir /S /Q game\\Builds\\Main\\Windows\\GameName_BackUpThisFolder_ButDontShipItWithYourGame'
    //       bat '"C:\\Program Files\\7-Zip\\7z" a game/Builds\\Main\\Windows\\gamename_windows_main.zip .\\game\\Builds\\Main\\Windows\\* || exit /b 1'
    //     }
    //   }
    // }
    // stage('Archiving Artifacts') {
    //   steps {
    //     archiveArtifacts artifacts: 'game/Builds/Main/Windows/gamename_windows_main.zip', onlyIfSuccessful: true
    //     // Delete the zip file because it has been archived and moved to Jenkins and because when we deploy to steam we dont want to include the zip
    //     bat 'del /Q game\\Builds\\Main\\Windows\\gamename_windows_main.zip'
    //   }
    // }
  }
}

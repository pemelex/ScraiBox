# INSTRUCTIONS
If you need full code of any file, reply with: <!cmd:scry:relative_path>

# PROJECT JSON MAP
{
  "Name": "ScraiBox",
  "Type": "Folder",
  "Path": ".",
  "Children": [
    {
      "Name": "ScraiBox.Console",
      "Type": "Folder",
      "Path": "ScraiBox.Console",
      "Children": [
        {
          "Name": "Program.cs",
          "Type": "File",
          "Path": "ScraiBox.Console\\Program.cs",
          "Children": [],
          "Signatures": []
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.Core",
      "Type": "Folder",
      "Path": "ScraiBox.Core",
      "Children": [
        {
          "Name": "CommandInterceptor.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\CommandInterceptor.cs",
          "Children": [],
          "Signatures": [
            "InterceptedCommand()",
            "InterceptAll()"
          ]
        },
        {
          "Name": "ContextScryer.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\ContextScryer.cs",
          "Children": [],
          "Signatures": [
            "Scry()",
            "ScryMultiple()",
            "IsExcluded()",
            "AppendFileContent()"
          ]
        },
        {
          "Name": "ProjectMapper.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\ProjectMapper.cs",
          "Children": [],
          "Signatures": [
            "GenerateJsonMap()",
            "ScanDirectory()",
            "ExtractSignatures()"
          ]
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.Gui",
      "Type": "Folder",
      "Path": "ScraiBox.Gui",
      "Children": [
        {
          "Name": "Components",
          "Type": "Folder",
          "Path": "ScraiBox.Gui\\Components",
          "Children": [
            {
              "Name": "Layout",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Components\\Layout",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "Pages",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Components\\Pages",
              "Children": [],
              "Signatures": []
            }
          ],
          "Signatures": []
        },
        {
          "Name": "Platforms",
          "Type": "Folder",
          "Path": "ScraiBox.Gui\\Platforms",
          "Children": [
            {
              "Name": "Android",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Platforms\\Android",
              "Children": [
                {
                  "Name": "Resources",
                  "Type": "Folder",
                  "Path": "ScraiBox.Gui\\Platforms\\Android\\Resources",
                  "Children": [
                    {
                      "Name": "values",
                      "Type": "Folder",
                      "Path": "ScraiBox.Gui\\Platforms\\Android\\Resources\\values",
                      "Children": [],
                      "Signatures": []
                    }
                  ],
                  "Signatures": []
                },
                {
                  "Name": "MainActivity.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\Android\\MainActivity.cs",
                  "Children": [],
                  "Signatures": []
                },
                {
                  "Name": "MainApplication.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\Android\\MainApplication.cs",
                  "Children": [],
                  "Signatures": []
                }
              ],
              "Signatures": []
            },
            {
              "Name": "iOS",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Platforms\\iOS",
              "Children": [
                {
                  "Name": "Resources",
                  "Type": "Folder",
                  "Path": "ScraiBox.Gui\\Platforms\\iOS\\Resources",
                  "Children": [],
                  "Signatures": []
                },
                {
                  "Name": "AppDelegate.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\iOS\\AppDelegate.cs",
                  "Children": [],
                  "Signatures": []
                },
                {
                  "Name": "Program.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\iOS\\Program.cs",
                  "Children": [],
                  "Signatures": [
                    "Main()"
                  ]
                }
              ],
              "Signatures": []
            },
            {
              "Name": "MacCatalyst",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Platforms\\MacCatalyst",
              "Children": [
                {
                  "Name": "AppDelegate.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\MacCatalyst\\AppDelegate.cs",
                  "Children": [],
                  "Signatures": []
                },
                {
                  "Name": "Program.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\MacCatalyst\\Program.cs",
                  "Children": [],
                  "Signatures": [
                    "Main()"
                  ]
                }
              ],
              "Signatures": []
            },
            {
              "Name": "Windows",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Platforms\\Windows",
              "Children": [
                {
                  "Name": "App.xaml.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Platforms\\Windows\\App.xaml.cs",
                  "Children": [],
                  "Signatures": []
                }
              ],
              "Signatures": []
            }
          ],
          "Signatures": []
        },
        {
          "Name": "Properties",
          "Type": "Folder",
          "Path": "ScraiBox.Gui\\Properties",
          "Children": [],
          "Signatures": []
        },
        {
          "Name": "Resources",
          "Type": "Folder",
          "Path": "ScraiBox.Gui\\Resources",
          "Children": [
            {
              "Name": "AppIcon",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Resources\\AppIcon",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "Fonts",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Resources\\Fonts",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "Images",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Resources\\Images",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "Raw",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Resources\\Raw",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "Splash",
              "Type": "Folder",
              "Path": "ScraiBox.Gui\\Resources\\Splash",
              "Children": [],
              "Signatures": []
            }
          ],
          "Signatures": []
        },
        {
          "Name": "wwwroot",
          "Type": "Folder",
          "Path": "ScraiBox.Gui\\wwwroot",
          "Children": [],
          "Signatures": []
        },
        {
          "Name": "App.xaml.cs",
          "Type": "File",
          "Path": "ScraiBox.Gui\\App.xaml.cs",
          "Children": [],
          "Signatures": []
        },
        {
          "Name": "MainPage.xaml.cs",
          "Type": "File",
          "Path": "ScraiBox.Gui\\MainPage.xaml.cs",
          "Children": [],
          "Signatures": []
        },
        {
          "Name": "MauiProgram.cs",
          "Type": "File",
          "Path": "ScraiBox.Gui\\MauiProgram.cs",
          "Children": [],
          "Signatures": [
            "CreateMauiApp()"
          ]
        }
      ],
      "Signatures": []
    }
  ],
  "Signatures": []
}
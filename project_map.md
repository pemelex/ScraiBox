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
          "Name": "InventoryService.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\InventoryService.cs",
          "Children": [],
          "Signatures": [
            "BuildInventory()",
            "Search()",
            "FindFilesByClassNames()",
            "GetProjectName()"
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
        },
        {
          "Name": "RoslynService.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\RoslynService.cs",
          "Children": [],
          "Signatures": [
            "GetUsedTypeNamesAsync()",
            "ExtractActualTypes()",
            "GetMethodCallsAsync()",
            "GetMethodSourceCodeAsync()",
            "IsBasicType()"
          ]
        },
        {
          "Name": "ScraiBoxPluginExtensions.cs",
          "Type": "File",
          "Path": "ScraiBox.Core\\ScraiBoxPluginExtensions.cs",
          "Children": [],
          "Signatures": [
            "AddScraiBoxPlugins()"
          ]
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.Core.Interfaces",
      "Type": "Folder",
      "Path": "ScraiBox.Core.Interfaces",
      "Children": [
        {
          "Name": "DTO",
          "Type": "Folder",
          "Path": "ScraiBox.Core.Interfaces\\DTO",
          "Children": [
            {
              "Name": "FileEntry.cs",
              "Type": "File",
              "Path": "ScraiBox.Core.Interfaces\\DTO\\FileEntry.cs",
              "Children": [],
              "Signatures": [
                "FileEntry()"
              ]
            },
            {
              "Name": "InterceptedCommand.cs",
              "Type": "File",
              "Path": "ScraiBox.Core.Interfaces\\DTO\\InterceptedCommand.cs",
              "Children": [],
              "Signatures": [
                "InterceptedCommand()"
              ]
            },
            {
              "Name": "ProjectInventory.cs",
              "Type": "File",
              "Path": "ScraiBox.Core.Interfaces\\DTO\\ProjectInventory.cs",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "ScraiBoxContext.cs",
              "Type": "File",
              "Path": "ScraiBox.Core.Interfaces\\DTO\\ScraiBoxContext.cs",
              "Children": [],
              "Signatures": []
            },
            {
              "Name": "ScraiBoxResult.cs",
              "Type": "File",
              "Path": "ScraiBox.Core.Interfaces\\DTO\\ScraiBoxResult.cs",
              "Children": [],
              "Signatures": []
            }
          ],
          "Signatures": []
        },
        {
          "Name": "IUseCasePlugin.cs",
          "Type": "File",
          "Path": "ScraiBox.Core.Interfaces\\IUseCasePlugin.cs",
          "Children": [],
          "Signatures": []
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
              "Children": [
                {
                  "Name": "Home.razor.cs",
                  "Type": "File",
                  "Path": "ScraiBox.Gui\\Components\\Pages\\Home.razor.cs",
                  "Children": [],
                  "Signatures": [
                    "PickFolder()",
                    "GenerateJsonMap()",
                    "ExecuteSelectedUseCase()",
                    "ProcessCommand()"
                  ]
                }
              ],
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
    },
    {
      "Name": "ScraiBox.Plugin.UC.Analysis",
      "Type": "Folder",
      "Path": "ScraiBox.Plugin.UC.Analysis",
      "Children": [
        {
          "Name": "DeepContextTracerUseCase.cs",
          "Type": "File",
          "Path": "ScraiBox.Plugin.UC.Analysis\\DeepContextTracerUseCase.cs",
          "Children": [],
          "Signatures": [
            "TraceCallsRecursive()"
          ]
        },
        {
          "Name": "MethodCallTreeUseCase.cs",
          "Type": "File",
          "Path": "ScraiBox.Plugin.UC.Analysis\\MethodCallTreeUseCase.cs",
          "Children": [],
          "Signatures": [
            "ExecuteAsync()",
            "TraceCallsRecursive()"
          ]
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.Plugin.UC.Implementation",
      "Type": "Folder",
      "Path": "ScraiBox.Plugin.UC.Implementation",
      "Children": [
        {
          "Name": "BlazorComponentEditUseCase.cs",
          "Type": "File",
          "Path": "ScraiBox.Plugin.UC.Implementation\\BlazorComponentEditUseCase.cs",
          "Children": [],
          "Signatures": [
            "ExecuteAsync()"
          ]
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.Plugin.UC.System",
      "Type": "Folder",
      "Path": "ScraiBox.Plugin.UC.System",
      "Children": [
        {
          "Name": "SelfHydrationUseCase.cs",
          "Type": "File",
          "Path": "ScraiBox.Plugin.UC.System\\SelfHydrationUseCase.cs",
          "Children": [],
          "Signatures": [
            "ExecuteAsync()"
          ]
        }
      ],
      "Signatures": []
    },
    {
      "Name": "ScraiBox.WinGui",
      "Type": "Folder",
      "Path": "ScraiBox.WinGui",
      "Children": [
        {
          "Name": "MainForm.cs",
          "Type": "File",
          "Path": "ScraiBox.WinGui\\MainForm.cs",
          "Children": [],
          "Signatures": [
            "btnBrowse_Click()",
            "btnProcess_Click()",
            "btnRunUseCase_Click()",
            "ProcessCommand()",
            "ExecuteUseCase()",
            "GenerateJsonMap()",
            "Log()"
          ]
        },
        {
          "Name": "MainForm.Designer.cs",
          "Type": "File",
          "Path": "ScraiBox.WinGui\\MainForm.Designer.cs",
          "Children": [],
          "Signatures": [
            "InitializeComponent()"
          ]
        },
        {
          "Name": "Program.cs",
          "Type": "File",
          "Path": "ScraiBox.WinGui\\Program.cs",
          "Children": [],
          "Signatures": [
            "Main()"
          ]
        }
      ],
      "Signatures": []
    }
  ],
  "Signatures": []
}
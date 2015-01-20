
Для работы с DirectX нужно:

- библиотеки d3dx9_30.dll, 
	Microsoft.DirectX.Direct3D.dll, Microsoft.DirectX.Direct3DX.dll, Microsoft.DirectX.dll
- в настройках компилятора указать платформу x86
- отключить исключение LoaderLock в Debug->Exceptions->Managed Debugging Assistances
- в конфиге проекта добавить 
	<startup useLegacyV2RuntimeActivationPolicy="true">    
	<supportedRuntime version="v2.0.50727"/>
	</startup>
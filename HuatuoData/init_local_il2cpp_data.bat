

rem ���޸�Ϊ������Unity��il2cppĿ¼
rem һ�������� C:\Program Files\Unity\Hub\Editor\2020.3.33f1c2\Editor\Data\il2cpp

set IL2CPP_PATH=C:\Program Files\Unity\Hub\Editor\2020.3.33f1c2\Editor\Data\il2cpp

if not exist "%IL2CPP_PATH%" (
    echo "��δָ����ȷ��il2cpp·��"
    goto EXIT
)

set LOCAL_IL2CPP_DATA=LocalIl2CppData

if not exist %LOCAL_IL2CPP_DATA% (
    mkdir %LOCAL_IL2CPP_DATA%
)


rem Unity ���ʱ����ʹ�û�������UNITY_IL2CPP_PATH�Զ���%IL2CPP_PATH%��λ��
rem ��ͬʱ��Ҫ��ͬ��Ŀ¼����MonoBleedingEdge�������Ҫ����������Ŀ¼

rem ���� MonoBleedingEdge Ŀ¼
set MBE=%LOCAL_IL2CPP_DATA%\MonoBleedingEdge
if not exist %MBE% (
    xcopy /q /i /e "%IL2CPP_PATH%\..\MonoBleedingEdge" %MBE%
)


rem ����il2cppĿ¼
set IL2CPP=%LOCAL_IL2CPP_DATA%\il2cpp
if not exist %IL2CPP% (
    xcopy /q /i /e "%IL2CPP_PATH%" %IL2CPP%
)

rem �������滻 il2cppĿ¼�µ�libil2cppΪ huatuo�޸ĺ�İ汾
rem ��Ҫʹ�� {https://gitee.com/juvenior/il2cpp_huatuo}/libil2cpp �滻 il2cpp/libil2cppĿ¼
rem ��Ҫʹ�� {https://gitee.com/focus-creative-games/huatuo}/huatuo ��ӵ� il2cpp/libil2cppĿ¼�£���il2cpp/libil2cpp/huatuo

set HUATUO_REPO=huatuo_repo

if not exist %HUATUO_REPO% (
    echo δ��װhuatuo https://gitee.com/focus-creative-games/huatuo,������ init_huatuo_repos.bat or init_huatuo_repos.sh
    goto EXIT 
)

set IL2CPP_HUATUO_REPO=il2cpp_huatuo_repo
if not exist %IL2CPP_HUATUO_REPO% (
    echo δ��װil2cpp_huatuo https://gitee.com/juvenior/il2cpp_huatuo ,������ init_huatuo_repos.bat or init_huatuo_repos.sh
    goto EXIT 
)

set LIBIL2CPP_PATH=%LOCAL_IL2CPP_DATA%\il2cpp\libil2cpp
rd /s /q %LIBIL2CPP_PATH%

xcopy /q /i /e %IL2CPP_HUATUO_REPO%\libil2cpp %LIBIL2CPP_PATH%
xcopy /q /i /e %HUATUO_REPO%\huatuo %LIBIL2CPP_PATH%\huatuo

echo ��ʼ���ɹ�

:EXIT

PAUSE
using System;
using DistractorProject.Transport;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ConnectionDataSettings))]
    public class ConnectionDataSettingsPropertyDrawer : PropertyDrawer
    {

        private const string RootElementPath = "Assets/UI/ConnectionDataSettingsPropertyDrawer.uxml";
        private VisualTreeAsset _rootAsset;
        private VisualElement _contentContainer;
        private VisualElement[] _contentElements;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!_rootAsset)
            {
                _rootAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(RootElementPath);
            }
            
            var rootElement = _rootAsset.Instantiate();
            var enumField = rootElement.Q<EnumField>();
            enumField.Init(NetworkEndpointSetting.AnyIPv4);
            

            var networkEndpointSetting = property.FindPropertyRelative(nameof(ConnectionDataSettings.endpointSource));
            enumField.bindingPath = nameof(ConnectionDataSettings.endpointSource);
            enumField.BindProperty(networkEndpointSetting);
            enumField.RegisterValueChangedCallback(OnNetworkEndpointSettingsChanged);
            enumField.value = (NetworkEndpointSetting)networkEndpointSetting.enumValueIndex;

            _contentContainer = rootElement.Q<VisualElement>("ContentArea");
            _contentElements = CreateContentContainers(property);
            
            //var enumValue = enumField.value is NetworkEndpointSetting value ? value : NetworkEndpointSetting.AnyIPv4;
            var chosenContainer = _contentElements[networkEndpointSetting.enumValueIndex];

            _contentContainer.Add(chosenContainer);
            
            
            
            return rootElement;
        }
        private VisualElement[] CreateContentContainers(SerializedProperty property)
        {
            var enumNames = Enum.GetNames(typeof(NetworkEndpointSetting));
            var result = new VisualElement[enumNames.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CreateContentContainerForEnumData(enumNames[i], property);
            }
            return result;
        }

        private VisualElement CreateContentContainerForEnumData(string enumName, SerializedProperty property)
        {
            switch (enumName)
            {
                case nameof(NetworkEndpointSetting.AnyIPv4): return CreateAnyIPv4Container(property);
                case nameof(NetworkEndpointSetting.AnyIPv6): return CreateAnyIPv6Container(property);
                case nameof(NetworkEndpointSetting.LoopbackIPv4): return CreateLoopbackIPv4Container(property);
                case nameof(NetworkEndpointSetting.LoopbackIPv6): return CreateLoopbackIPv6Container(property);
                case nameof(NetworkEndpointSetting.Custom): return CreateCustomContainer(property);
            }

            return new VisualElement();
        }

        
        //todo set the actual ip address to the correct thing?
        private VisualElement CreateAnyIPv4Container(SerializedProperty property)
        {
            return CreatePortField(property);
        }

        private static IntegerField CreatePortField(SerializedProperty property)
        {
            var portProperty = property.FindPropertyRelative(nameof(ConnectionDataSettings.port));
            var portField = new IntegerField("Port")
            {
                bindingPath = nameof(ConnectionDataSettings.port)
            };
            portField.maxLength = 4;
            portField.BindProperty(portProperty);
            return portField;
        }
        
        private VisualElement CreateAnyIPv6Container(SerializedProperty property)
        {
            return CreatePortField(property);
        }
        
        private VisualElement CreateLoopbackIPv4Container(SerializedProperty property)
        {
            return CreatePortField(property);
        }
        
        private VisualElement CreateLoopbackIPv6Container(SerializedProperty property)
        {
            return CreatePortField(property);
        }
        
        private VisualElement CreateCustomContainer(SerializedProperty property)
        {
            var ipAddressProperty = property.FindPropertyRelative(nameof(ConnectionDataSettings.ipAddress));
            return new PropertyField(ipAddressProperty);
        }

        

        private void OnNetworkEndpointSettingsChanged(ChangeEvent<Enum> evt)
        {
            if (evt.newValue is not NetworkEndpointSetting setting)
            {
                return;
            }
            _contentContainer.Clear();
            _contentContainer.Add(_contentElements[(int)setting]);
        }
    }
}
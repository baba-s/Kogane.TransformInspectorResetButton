using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    [CanEditMultipleObjects]
    [CustomEditor( typeof( Transform ) )]
    internal sealed class TransformInspectorResetButton : Editor
    {
        private Editor             m_editor;
        private SerializedProperty m_positionProperty;
        private SerializedProperty m_rotationProperty;
        private SerializedProperty m_scaleProperty;

        private void OnEnable()
        {
            m_positionProperty = serializedObject.FindProperty( "m_LocalPosition" );
            m_rotationProperty = serializedObject.FindProperty( "m_LocalRotation" );
            m_scaleProperty    = serializedObject.FindProperty( "m_LocalScale" );
        }

        private void OnDisable()
        {
            m_positionProperty = null;
            m_rotationProperty = null;
            m_scaleProperty    = null;
        }

        public override void OnInspectorGUI()
        {
            if ( m_editor == null )
            {
                var type = typeof( Editor ).Assembly.GetType( "UnityEditor.TransformInspector" );
                m_editor = CreateEditor( target, type );
            }

            m_editor.OnInspectorGUI();

            using var scope = new EditorGUILayout.HorizontalScope();

            EditorGUILayout.PrefixLabel( "Reset" );

            using ( new GUIEnabledScope( m_positionProperty.vector3Value != Vector3.zero ) )
            {
                if ( GUILayout.Button( "Position" ) )
                {
                    m_positionProperty.vector3Value = Vector3.zero;
                }
            }

            using ( new GUIEnabledScope( m_rotationProperty.quaternionValue != Quaternion.identity ) )
            {
                if ( GUILayout.Button( "Rotation" ) )
                {
                    m_rotationProperty.quaternionValue = Quaternion.identity;
                }
            }

            using ( new GUIEnabledScope( m_scaleProperty.vector3Value != Vector3.one ) )
            {
                if ( GUILayout.Button( "Scale" ) )
                {
                    m_scaleProperty.vector3Value = Vector3.one;
                }
            }

            using ( new GUIEnabledScope
                   (
                       m_positionProperty.vector3Value != Vector3.zero ||
                       m_rotationProperty.quaternionValue != Quaternion.identity ||
                       m_scaleProperty.vector3Value != Vector3.one
                   ) )
            {
                if ( GUILayout.Button( "All" ) )
                {
                    m_positionProperty.vector3Value    = Vector3.zero;
                    m_rotationProperty.quaternionValue = Quaternion.identity;
                    m_scaleProperty.vector3Value       = Vector3.one;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
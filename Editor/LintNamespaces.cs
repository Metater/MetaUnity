using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Metater.Editor
{
    [InitializeOnLoad]
    public static class LintNamespaces
    {
        static LintNamespaces()
        {
            Verify();
        }

        private static void Verify()
        {
            var paths = AssetDatabase.GetAllAssetPaths();
            paths = paths.Where(p => p.EndsWith(".cs") && (p.StartsWith("Assets/_Objects/") || p.StartsWith("Assets/Metater/"))).ToArray();

            foreach (var path in paths)
            {
                var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                if (textAsset == null)
                {
                    continue;
                }

                string expectedNamespace = path[..^(textAsset.name.Length + 4)].Replace('/', '.');
                if (!textAsset.text.Contains($"namespace {expectedNamespace}"))
                {
                    bool commentedOut = textAsset.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries).All(l => l.TrimStart().StartsWith("//") || string.IsNullOrWhiteSpace(l));
                    bool empty = string.IsNullOrWhiteSpace(textAsset.text);
                    if (commentedOut || empty)
                    {
                        continue;
                    }

                    Debug.LogWarning($"Incorrect namespace in {path}\nShould be {expectedNamespace}");
                }
            }
        }
    }
}
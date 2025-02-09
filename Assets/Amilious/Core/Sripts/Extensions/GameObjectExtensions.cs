/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the <see cref="GameObject"/> class.
    /// </summary>
    public static class GameObjectExtensions {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try get the components of the given type.
        /// </summary>
        /// <param name="gameObject">The GameObject that you are trying to get components of.</param>
        /// <param name="components">The found components of the given type.</param>
        /// <typeparam name="T">The type of the components.</typeparam>
        /// <returns>True if any components of the given type were found, otherwise
        /// returns false.</returns>
        public static bool TryGetComponents<T>(this GameObject gameObject, out IEnumerable<T> components) {
            if(gameObject == null) {
                components = default(T[]);
                return false;
            }
            components = gameObject.GetComponents<T>();
            return components == null || !components.Any();
        }
        
         /// <summary>
        /// This method is used to find a child <see cref="GameObject"/> with the given <see cref="name"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> containing the child you want to find.</param>
        /// <param name="name">The name of the child <see cref="GameObject"/> that you want to find.</param>
        /// <returns>The found <see cref="GameObject"/> or null.</returns>
        public static GameObject FindChild(this GameObject gameObject, string name) {
            return GameObject.Find($"{gameObject.transform.GetPath()}/{name}");
        }
        
        /// <summary>
        /// This method is used to get or add a child <see cref="GameObject"/> by name.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> that you want to find or create the child in.</param>
        /// <param name="name">The name of the child <see cref="GameObject"/>.</param>
        /// <returns>The found or created child <see cref="GameObject"/>.</returns>
        public static GameObject GetOrAddChild(this GameObject gameObject, string name) {
            var child = gameObject.FindChild(name);
            if(child != null) return child;
            child = new GameObject(name) {
                transform = {
                    parent = gameObject.transform,
                    localPosition = Vector3.zero,
                    localRotation = Quaternion.identity,
                    localScale = Vector3.one
                }
            };
            return child;
        }
        
        /// <summary>
        /// This method is used to get or add a component to the passed <see cref="gameObject"/>.
        /// </summary>
        /// <param name="gameObject">This is the <see cref="GameObject"/> that you want to get or add a component to.</param>
        /// <typeparam name="T">The type of the component that you want to get or add.</typeparam>
        /// <returns>The found or created <see cref="Component"/>.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            var c = gameObject.GetComponent<T>();
            return c != null ? c : gameObject.AddComponent<T>();
        }
        
        /// <summary>
        /// This method is used to destroy all of the <see cref="gameObject"/>s children.
        /// </summary>
        /// <param name="gameObject">The game object that you want to remove children from.</param>
        public static void DestroyChildren(this GameObject gameObject) {
            foreach(Transform child in gameObject.transform) Object.Destroy(child.gameObject);
        }

        /// <summary>
        /// This method is used to destroy all of the <see cref="gameObject"/>s children immediately.
        /// </summary>
        /// <param name="gameObject">The game object that you want to remove children from.</param>
        public static void DestroyChildrenImmediate(this GameObject gameObject) {
            foreach(Transform child in gameObject.transform) Object.DestroyImmediate(child.gameObject);
        }

        /// <summary>
        /// This method is used to destroy a GameObject's components of the given type.
        /// </summary>
        /// <param name="gameObject">The game object you want to destroy components on.</param>
        /// <typeparam name="T">The type of component that you want to destroy.</typeparam>
        public static void DestroyComponents<T>(this GameObject gameObject) where T : Component {
            foreach(var component in gameObject.GetComponents<T>()) {
                if(Application.isEditor)Object.DestroyImmediate(component);
                else Object.Destroy(component);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
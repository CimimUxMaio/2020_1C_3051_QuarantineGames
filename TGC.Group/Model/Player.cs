﻿using Microsoft.DirectX.DirectInput;
using System.Diagnostics;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Player : GameObject
    {
        /* STATS */
        private float movementSpeed = 130.0f;

        public Player(Subnautica gameInstance, string name) : base(gameInstance, name)
        {
            var loader = new TgcSceneLoader();
            var playerScene = loader.loadSceneFromFile(GameInstance.MediaDir + "Player\\Player-TgcScene.xml");
            Mesh = playerScene.Meshes[0];
            Mesh.Scale = new TGCVector3(0.1f, 0.1f, 0.1f);
            Mesh.Position = new TGCVector3(0, 100, -1000);
            LookDirection = new TGCVector3(0, 0, -1);  // Por como esta orientado el Mesh originalmente
        }

        #region GameObject

        public override void Update()
        {
            ManageMovement();
        }

        public override void Render()
        {
            Mesh.Render();
        }

        public override void Dispose()
        {
            Mesh.Dispose();
        }

        #endregion

        #region PRIVATE_METHODS

        // <summary>
        //      Dado un input WASD el Mesh se mueve a la izquierda, derecha, adelante y atras respecto del vector LookDirection.
        // <summary>
        private void ManageMovement()
        {
            TgcD3dInput input = GameInstance.Input;
            TGCVector3 movementDirection = TGCVector3.Empty;

            if (input.keyDown(Key.W))  // Adelante
            {
                movementDirection = LookDirection;
            }
            else if (input.keyDown(Key.S))  // Atras
            {
                movementDirection = -LookDirection;
            }
            else if (input.keyDown(Key.A))  // Izquierda
            {
                TGCMatrix rotationMatrix = TGCMatrix.RotationY(FastMath.PI_HALF);
                movementDirection = ApplyTransformation(rotationMatrix, LookDirection);
            }
            else if (input.keyDown(Key.D))  // Derecha
            {
                TGCMatrix rotationMatrix = TGCMatrix.RotationY(FastMath.PI_HALF * 3);
                movementDirection = ApplyTransformation(rotationMatrix, LookDirection);
            }
            else if (input.keyDown(Key.Space))    // De acá para abajo son solo de prueba para poder moverme en la escena libremente
            {
                movementDirection = TGCVector3.Up;
            }
            else if (input.keyDown(Key.X))
            {
                movementDirection = -TGCVector3.Up;
            }
            else if (input.keyDown(Key.Q))   // No funca
            {
                System.Console.WriteLine("Q pressed");
                System.Console.WriteLine("Previous rotation: " + Mesh.Rotation);
                Mesh.Rotation -= new TGCVector3(0, 1000 * GameInstance.ElapsedTime, 0);
                Mesh.Transform = TGCMatrix.RotationY(Mesh.Rotation.Y);
                System.Console.WriteLine("Final rotation: " + Mesh.Rotation);
            }

            if (!CollisionDetected())
            {
                Translate(movementDirection * movementSpeed * GameInstance.ElapsedTime);
            }
        }

        // <summary>
        //      Multiplico la matriz por el vector a transformar.
        // <summary>
        private TGCVector3 ApplyTransformation(TGCMatrix rotationMatrix, TGCVector3 vector)
        {
            TGCVector3 result = TGCVector3.Empty;
            result.X = rotationMatrix.M11 * vector.X + rotationMatrix.M12 * vector.Y + rotationMatrix.M13 * vector.Z;
            result.Y = rotationMatrix.M21 * vector.X + rotationMatrix.M22 * vector.Y + rotationMatrix.M23 * vector.Z;
            result.Z = rotationMatrix.M31 * vector.X + rotationMatrix.M32 * vector.Y + rotationMatrix.M33 * vector.Z;

            return result;
        }

        private bool CollisionDetected() => false;  // TODO Tal vez podria ir en la clase GameObject

        #endregion

        #region PUBLIC_METHODS


        #endregion
    }
}
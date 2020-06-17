﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vorpadminmenu_cl.Functions.Utils;

namespace vorpadminmenu_cl.Functions.Teleports
{
    class TeleportsFunctions : BaseScript
    {
        public static Vector3 lastTpCoords = new Vector3(0.0F, 0.0F, 0.0F);
        static bool guarma = false;
        public static bool tpView;
        public static bool deleteOn = false;
        
        
        public TeleportsFunctions()
        {
            
            EventHandlers["vorp:sendCoordsToDestinyBring"] += new Action<Vector3>(Bring);
            EventHandlers["vorp:askForCoords"] += new Action<string>(ResponseCoords);
            EventHandlers["vorp:coordsToStart"] += new Action<Vector3>(TpToPlayerDone);



            Tick += OnView;
            Tick += OnTpView;

            

            API.RegisterCommand("tpwayp", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpToWaypoint(args);
                }
            }), false);

            API.RegisterCommand("tpcoords", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpToCoords(args);
                }
            }), false);

            API.RegisterCommand("tpplayer", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpToPlayer(args);
                }
            }), false);

            API.RegisterCommand("tpbring", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpBring(args);
                }
            }), false);

            API.RegisterCommand("tpback", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpBack(args);
                }
            }), false);

            API.RegisterCommand("delback", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    DelBack(args);
                }
            }), false);

            API.RegisterCommand("guarma", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    Guarma(args);
                }
            }), false);

            API.RegisterCommand("tpview", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (GetUserInfo.userGroup != "user")
                {
                    TpView(args);
                }
            }), false);
            
        }


    
        public static async void TpToWaypoint(List<object> args)
        {
            Vector3 waypointCoords = API.GetWaypointCoords();
            if (waypointCoords.X != 0.0f && waypointCoords.Y != 0.0f && waypointCoords.Z != 0.0f) {
                if (UtilsFunctions.blip == -1)
                {
                    lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                    UtilsFunctions.CreateBlip();
                }
                await UtilsFunctions.TeleportAndFoundGroundAsync(waypointCoords);
            }
        }

        public static async void TpToCoords(List<object> args)
        {
            try
            {
                if (UtilsFunctions.blip == -1)
                {
                    lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                    UtilsFunctions.CreateBlip();
                }
                float XCoord = float.Parse(args[0].ToString());
                float YCoord = float.Parse(args[1].ToString());
                float ZCoord = 0.0f;
                Vector3 chosenCoords = new Vector3(XCoord, YCoord, ZCoord);
                await UtilsFunctions.TeleportAndFoundGroundAsync(chosenCoords);

            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public static void TpBring(List<object> args)
        {
            int destinataryID = int.Parse(args[0].ToString());
            Vector3 ownCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);

            TriggerServerEvent("vorp:ownerCoordsToBring", ownCoords, destinataryID);
        }

        public void Bring(Vector3 bringCoords)
        {
            UtilsFunctions.TeleportToCoords(bringCoords.X, bringCoords.Y, bringCoords.Z);
        }

        public static void TpToPlayer(List<object> args)
        {
            if (UtilsFunctions.blip == -1)
            {
                lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                UtilsFunctions.CreateBlip();
            }
            int destinataryID = int.Parse(args[0].ToString());
            TriggerServerEvent("vorp:askCoordsToTPPlayerDestiny", destinataryID);
        }

        private void ResponseCoords(string sourceID)
        {
            Vector3 responseCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            TriggerServerEvent("vorp:callbackCoords", sourceID, responseCoords);
        }


        private void TpToPlayerDone(Vector3 coordsToTp)
        {
            UtilsFunctions.TeleportToCoords(coordsToTp.X, coordsToTp.Y, coordsToTp.Z);
        }

        public static void TpBack(List<object> args)
        {
            if (UtilsFunctions.blip != -1)
            {
                API.RemoveBlip(ref UtilsFunctions.blip);
                UtilsFunctions.blip = -1;
                UtilsFunctions.TeleportToCoords(lastTpCoords.X, lastTpCoords.Y, lastTpCoords.Z);
            }
        }

        public static void DelBack(List<object> args)
        {
            API.RemoveBlip(ref UtilsFunctions.blip);
            UtilsFunctions.blip = -1;
            lastTpCoords = new Vector3(0.0F, 0.0F, 0.0F);
        }

        public static void Guarma(List<object> args)
        {

            if (!guarma)
            {
                if (UtilsFunctions.blip == -1)
                {
                    lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                    UtilsFunctions.CreateBlip();
                }
                API.SetEntityCoords(API.PlayerPedId(), 1471.457F, -7128.961F, 75.80013F, false, false, false, false);
                Function.Call((Hash)0xA657EC9DBC6CC900, 1935063277);
                Function.Call((Hash)0xE8770EE02AEE45C2, 1);
                Function.Call((Hash)0x74E2261D2A66849A, true);
                guarma = true;
            }
            else
            {
                TpBack(args);
                Function.Call((Hash)0xA657EC9DBC6CC900, -1868977180);
                Function.Call((Hash)0xE8770EE02AEE45C2, 0);
                Function.Call((Hash)0x74E2261D2A66849A, false);
                guarma = false;
            }

        }

        [Tick]
        public async Task OnTpView()
        {
            await Delay(0);
            int entity = 0;
            bool hit = false;
            Vector3 endCoord = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            Vector3 camCoords = API.GetGameplayCamCoord();
            Vector3 sourceCoords = UtilsFunctions.GetCoordsFromCam(100000.0F);
            int rayHandle = API.StartShapeTestRay(camCoords.X, camCoords.Y, camCoords.Z, sourceCoords.X, sourceCoords.Y, sourceCoords.Z, -1, API.PlayerPedId(), 0);
            API.GetShapeTestResult(rayHandle, ref hit, ref endCoord, ref surfaceNormal, ref entity);



            if (API.IsControlJustPressed(0, 0xCEE12B50) && tpView && endCoord.X != 0.0)
            {
                Vector3 waypointCoords = API.GetWaypointCoords();
                if (UtilsFunctions.blip == -1)
                {
                    lastTpCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                    UtilsFunctions.CreateBlip();
                }
                UtilsFunctions.TeleportToCoords(endCoord.X, endCoord.Y, endCoord.Z);
            }
        }



        [Tick]
        public async Task OnView()
        {
            int entity = 0;
            bool hit = false;
            Vector3 endCoord = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            Vector3 camCoords = API.GetGameplayCamCoord();
            Vector3 sourceCoords = UtilsFunctions.GetCoordsFromCam(1000.0F);
            int rayHandle = API.StartShapeTestRay(camCoords.X, camCoords.Y, camCoords.Z, sourceCoords.X, sourceCoords.Y, sourceCoords.Z, -1, API.PlayerPedId(), 0);
            API.GetShapeTestResult(rayHandle, ref hit, ref endCoord, ref surfaceNormal, ref entity);
            if (tpView)
            {
                Function.Call((Hash)0x2A32FAA57B937173, -1795314153, endCoord.X, endCoord.Y, endCoord.Z, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.5F, 0.5F, 50.0F, 0, 255, 0, 155, false, false, 2, false, 0, 0, false);
            }
        }


        public void TpView(List<object> args)
        {
            if (tpView)
            {
                Function.Call(Hash.SET_PLAYER_INVINCIBLE, API.PlayerId(), false);
                tpView = false;
            }
            else
            {
                Function.Call(Hash.SET_PLAYER_INVINCIBLE, API.PlayerId(), true);
                tpView = true;
            }
        }
    }

}
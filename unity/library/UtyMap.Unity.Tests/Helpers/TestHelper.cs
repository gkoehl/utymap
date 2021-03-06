﻿using System;
using UtyMap.Unity.Core;
using UtyMap.Unity.Infrastructure.Config;
using UtyMap.Unity.Infrastructure.Diagnostic;
using UtyMap.Unity.Infrastructure.IO;
using UtyDepend;
using UtyDepend.Config;
using UtyMap.Unity.Maps.Data;

namespace UtyMap.Unity.Tests.Helpers
{
    internal static class TestHelper
    {
        #region Constants values

        public const string IntegrationTestCategory = "Integration";

        public static GeoCoordinate WorldZeroPoint = new GeoCoordinate(52.5317429, 13.3871987);

        public const string TestAssetsFolder = @"../../../../demo/Assets/StreamingAssets";
        
        public const string BerlinXmlData = TestAssetsFolder + @"/Osm/berlin.osm.xml";
        public const string BerlinPbfData = TestAssetsFolder + @"/Osm/berlin.osm.pbf";
        public const string DefaultMapCss = TestAssetsFolder + @"/MapCss/default/default.mapcss";

        public const string NaturalEarth110mAdmin = TestAssetsFolder + @"/NaturalEarth/ne_110m_admin_0_scale_rank";
        public const string NaturalEarth110mLakes = TestAssetsFolder + @"/NaturalEarth/ne_110m_lakes";
        public const string NaturalEarth110mLand = TestAssetsFolder + @"/NaturalEarth/ne_110m_land";
        public const string NaturalEarth110mRivers = TestAssetsFolder + @"/NaturalEarth/ne_110m_rivers_lake_centerlines";

        #endregion

        public static CompositionRoot GetCompositionRoot(GeoCoordinate worldZeroPoint)
        {
            return GetCompositionRoot(worldZeroPoint, (container, section) => { });
        }

        public static CompositionRoot GetCompositionRoot(GeoCoordinate worldZeroPoint,
            Action<IContainer, IConfigSection> action)
        {
            // create default container which should not be exposed outside
            // to avoid Service Locator pattern.
            IContainer container = new Container();

            // create default application configuration
            var config = ConfigBuilder.GetDefault()
                .SetStringIndex("Index/")
                .SetSpatialIndex("Index/")
                .SetElevationType(ElevationDataType.Flat)
                .Build();

            // initialize services
            return new CompositionRoot(container, config)
                .RegisterAction((c, _) => c.Register(Component.For<ITrace>().Use<ConsoleTrace>()))
                .RegisterAction((c, _) => c.Register(Component.For<IPathResolver>().Use<TestPathResolver>()))
                .RegisterAction((c, _) => c.Register(Component.For<Stylesheet>().Use<Stylesheet>(DefaultMapCss)))
                .RegisterAction((c, _) => c.Register(Component.For<IProjection>().Use<CartesianProjection>(worldZeroPoint)))
                .RegisterAction(action)
                .Setup();
        }
    }
}

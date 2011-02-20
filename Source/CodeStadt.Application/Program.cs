﻿namespace CodeStadt.Application
{
    using System;
    using System.IO;
    using System.Linq;
    using CodeStadt.Draw.RayTracer;
    using CodeStadt.Draw.RayTracer.Environment;
    using CodeStadt.Draw.RayTracer.Environment.Objects;
    using Driven.Metrics.metrics;
    using Driven.Metrics.Reporting;

    class Program
    {
        /// <summary>
        /// Application main method.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {

            CommandLineArguments cArgs = new CommandLineArguments(args);

            Configuration config = new Configuration();

            var something = new Driven.Metrics.DrivenMetrics.Factory().Create(
                 cArgs.Assemblies.ToArray(),
                 config.Container.ResolveAll<IMetricCalculator>() ,
                     "TestReport",
                 config.Container.Resolve<IReport>());

            something.RunAllMetricsAndGenerateReport();

            CodeStadt.Core.DrivenMetrics.Reporting.ResultOutput results = something.Report.As<Core.DrivenMetrics.Reporting.ResultOutput>();

            if (results != null)
            {
                results.Results.ForEach(x =>
                {
                    Console.WriteLine("Metric: {0}".Formatted(x.Name));
                    Console.WriteLine("");

                    x.ClassResults.ForEach(y =>
                    {
                        Console.WriteLine("  Class: {0}".Formatted(y.Name));
                        Console.WriteLine("");
                        y.MethodResults.ForEach(z =>
                        {
                            Console.WriteLine("    Method: {0} \n\r    Result: {1}".Formatted(z.Name, z.Result));
                            Console.WriteLine("");

                        });
                    });
                });
            }

            // Console.ReadLine();

            Console.WriteLine("Going to try and draw an image :-)");

            // Lets ray trace something
            string raytraceFileName = "raytrace.jpg";
            if (File.Exists(raytraceFileName)) File.Delete(raytraceFileName);

            int width = 600;
            int height = 600;
            var bitmap = new System.Drawing.Bitmap(width, height);
            

            RayTracer rayTracer = new RayTracer(width, height, (int x, int y, System.Drawing.Color color) =>
            {
                bitmap.SetPixel(x, y, color);
            });

            var MyScene = new Scene()
            {
                Elements = new SceneObject[] { 
                                new Plane() {
                                    Norm = new Vector(0,1,0),
                                    Point = new Vector(0,0,0),
                                    Surface = Surfaces.CheckerBoard
                                },
                                new Sphere() {
                                    Center = new Vector(0,1,0),
                                    Radius = 1,
                                    Surface = Surfaces.Shiny
                                },
                                new Sphere() {
                                    Center = new Vector(-1,.5,1.5),
                                    Radius = .5,
                                    Surface = Surfaces.Shiny
                                }},
                Lights = new Light[] { 
                                new Light() {
                                    Position = new Vector(-2,2.5,0),
                                    Color = new Color(.49,.07,.07)
                                },
                                new Light() {
                                    Position = new Vector(1.5,2.5,1.5),
                                    Color = new Color(.07,.07,.49)
                                },
                                new Light() {
                                    Position = new Vector(1.5,2.5,-1.5),
                                    Color = new Color(.07,.49,.071)
                                },
                                new Light() {
                                    Position = new Vector(0,3.5,0),
                                    Color = new Color(.21,.21,.35)
                                }},
                Camera = new Camera(new Vector(3, 2, 4), new Vector(-1, .5, 0))
            };

            rayTracer.Render(MyScene);

            bitmap.Save(raytraceFileName);
        }
    }
}

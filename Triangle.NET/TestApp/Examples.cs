﻿// -----------------------------------------------------------------------
// <copyright file="Examples.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet;
    using TriangleNet.IO;

    /// <summary>
    /// Code of the online examples.
    /// 
    /// </summary>
    public static class Examples
    {
        // Make sure this path points to the polygon sample data.
        static readonly string pathToData = @"..\..\..\Data\";

        /// <summary>
        /// Generating Delaunay triangulations
        /// </summary>
        public static void Example1()
        {
            ImageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Read spiral node file and gernerate the delaunay triangulation 
            // of the point set.
            mesh.Triangulate(pathToData + "spiral.node");
            ImageWriter.WritePng(mesh, "spiral.png", 180);

            // Read face polygon file and gernerate the delaunay triangulation 
            // of the PSLG. We reuse the mesh instance here.
            MeshData data = FileReader.ReadFile(pathToData + "face.poly");
            mesh.Triangulate(data);
            ImageWriter.WritePng(mesh, "face.png", 200);

            // Generate a conforming delaunay triangulation of the face polygon.
            mesh.SetOption(Options.ConformingDelaunay, true);
            mesh.Triangulate(data);
            ImageWriter.WritePng(mesh, "face-CDT.png", 200);
        }

        /// <summary>
        /// Quality meshing: angle and size constraints
        /// </summary>
        public static void Example2()
        {
            ImageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Read spiral node file and gernerate the delaunay triangulation. 
            // Set the mesh quality option to true, which will set a default
            // minimum angle of 20 degrees.
            MeshData data = FileReader.ReadNodeFile(pathToData + "spiral.node");
            mesh.SetOption(Options.Quality, true);
            mesh.Triangulate(data);
            ImageWriter.WritePng(mesh, "spiral-Angle-20.png", 200);

            // Set a minimum angle of 30 degrees. 
            mesh.SetOption(Options.MinAngle, 35);
            mesh.Triangulate(data);
            ImageWriter.WritePng(mesh, "spiral-Angle-35.png", 200);

            // Reset the minimum angle and add a global area constraint.
            mesh.SetOption(Options.MinAngle, 20);
            mesh.SetOption(Options.MaxArea, 0.2);
            mesh.Triangulate(data);
            ImageWriter.WritePng(mesh, "spiral-Area.png", 200);
        }

        /// <summary>
        /// Refining preexisting meshes
        /// </summary>
        public static void Example3()
        {
            ImageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Gernerate a quality delaunay triangulation of box
            // polygon, containing the convex hull.
            mesh.SetOption(Options.Quality, true);
            mesh.SetOption(Options.Convex, true);
            mesh.Triangulate(pathToData + "box.poly");
            ImageWriter.WritePng(mesh, "box.png", 200);

            // Save the current mesh to .node and .ele files
            FileWriter.WriteNodes(mesh, "box.1.node");
            FileWriter.WriteElements(mesh, "box.1.ele");

            // Refine the mesh by setting a global area constraint.
            mesh.Refine(0.2);
            ImageWriter.WritePng(mesh, "box-Refine-1.png", 200);

            // Refine again by setting a smaller area constraint.
            mesh.Refine(0.05);
            ImageWriter.WritePng(mesh, "box-Refine-2.png", 200);

            // Load the previously saved box.1 mesh. Since a box.1.area
            // file exist, the variable area constraint option is set
            // and will be applied for refinement.
            mesh.Load(pathToData + "box.1.node");
            mesh.SetOption(Options.MinAngle, 0);
            mesh.Refine();
            ImageWriter.WritePng(mesh, "box-Refine-3.png", 200);
        }
    }
}
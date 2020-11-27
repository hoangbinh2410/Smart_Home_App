using Android.App;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.View;

using System.Collections.Generic;

using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;
using NativeBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;

namespace BA_MobileGPS.Core.Droid
{
    public class ClusterRenderer : DefaultClusterRenderer
    {
        private readonly Map map;
        private readonly Dictionary<string, NativeBitmapDescriptor> disabledBucketsCache;
        private readonly Dictionary<string, NativeBitmapDescriptor> enabledBucketsCache;
        private Dictionary<int, NativeBitmapDescriptor> mIcons;
        private IconClusterGenerator mIconGenerator;
        private ShapeDrawable mColoredCircleBackground;

        /**
 * If cluster size is less than this size, display individual markers.
 */
        private static int MIN_CLUSTER_SIZE = 4;

        public ClusterRenderer(Activity context,
            Map map,
            Android.Gms.Maps.GoogleMap nativeMap,
            ClusterManager manager)
            : base(context, nativeMap, manager)
        {
            this.map = map;
            MinClusterSize = map.ClusterOptions.MinimumClusterSize;
            disabledBucketsCache = new Dictionary<string, NativeBitmapDescriptor>();
            enabledBucketsCache = new Dictionary<string, NativeBitmapDescriptor>();
            mIcons = new Dictionary<int, NativeBitmapDescriptor>();
            mIconGenerator = new IconClusterGenerator(context);
            mIconGenerator.SetBackground(makeClusterBackground());
        }

        protected override bool ShouldRenderAsCluster(ICluster cluster)
        {
            return cluster.Size > MIN_CLUSTER_SIZE * 2;
        }

        public void SetUpdateMarker(ClusteredMarker clusteredMarker)
        {
            var marker = GetMarker(clusteredMarker);
            if (marker == null) return;
            marker.SetIcon(clusteredMarker.Icon);
        }

        public void SetUpdatePositionMarker(ClusteredMarker clusteredMarker)
        {
            var marker = GetMarker(clusteredMarker);
            if (marker == null) return;
            marker.Position = new LatLng(clusteredMarker.Position.Latitude, clusteredMarker.Position.Longitude);
        }

        public void SetUpdateRotationMarker(ClusteredMarker clusteredMarker)
        {
            var marker = GetMarker(clusteredMarker);
            if (marker == null) return;
            marker.Rotation = clusteredMarker.Rotation;
        }

        protected override void OnBeforeClusterRendered(ICluster cluster, MarkerOptions options)
        {
            NativeBitmapDescriptor icon;
            if (map.ClusterOptions.RendererCallback != null)
            {
                var descriptorFromCallback =
                    map.ClusterOptions.RendererCallback(map.ClusterOptions.EnableBuckets ?
                        GetClusterText(cluster) : cluster.Size.ToString());
                icon = GetIcon(cluster, descriptorFromCallback);
                options.SetIcon(icon);
            }
            else if (map.ClusterOptions.RendererImage != null)
            {
                icon = GetIcon(cluster, map.ClusterOptions.RendererImage);
                options.SetIcon(icon);
            }
            else
                base.OnBeforeClusterRendered(cluster, options);
        }

        private NativeBitmapDescriptor GetIcon(ICluster cluster, BitmapDescriptor descriptor)
        {
            var icon = GetFromIconCache(cluster);
            if (icon == null)
            {
                try
                {
                    int bucket = GetBucket(cluster);
                    var exists = mIcons.ContainsKey(bucket);
                    if (exists)
                    {
                        icon = mIcons[bucket];
                    }
                    else
                    {
                        mColoredCircleBackground.Paint.Color = Color.White;
                        icon = AndroidBitmapDescriptorFactory.FromBitmap(mIconGenerator.MakeIcon(bucket.ToString()));
                        mIcons.Add(bucket, icon);
                    }
                    AddToIconCache(cluster, icon);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            return icon;
        }

        private NativeBitmapDescriptor GetFromIconCache(ICluster cluster)
        {
            NativeBitmapDescriptor bitmapDescriptor;
            var clusterText = GetClusterText(cluster);
            if (map.ClusterOptions.EnableBuckets)
                enabledBucketsCache.TryGetValue(clusterText, out bitmapDescriptor);
            else
                disabledBucketsCache.TryGetValue(clusterText, out bitmapDescriptor);
            return bitmapDescriptor;
        }

        private void AddToIconCache(ICluster cluster, NativeBitmapDescriptor icon)
        {
            var clusterText = GetClusterText(cluster);
            if (map.ClusterOptions.EnableBuckets)
                enabledBucketsCache.Add(clusterText, icon);
            else
                disabledBucketsCache.Add(clusterText, icon);
        }

        protected override void OnBeforeClusterItemRendered(Java.Lang.Object marker, MarkerOptions options)
        {
            var clusteredMarker = marker as ClusteredMarker;

            options.SetTitle(clusteredMarker.Title)
                .SetSnippet(clusteredMarker.Snippet)
                .Draggable(clusteredMarker.Draggable)
                .SetRotation(clusteredMarker.Rotation)
                .Anchor(clusteredMarker.AnchorX, clusteredMarker.AnchorY)
                .SetAlpha(clusteredMarker.Alpha)
                .Flat(clusteredMarker.Flat);

            if (clusteredMarker.Icon != null)
                options.SetIcon(clusteredMarker.Icon);

            base.OnBeforeClusterItemRendered(marker, options);
        }

        protected override int GetBucket(ICluster cluster)
        {
            return cluster.Size / 2;
        }

        //protected override int GetColor(int size)
        //{
        //    return map.ClusterOptions.BucketColors[BucketIndexForSize(size)].ToAndroid();
        //}

        private string GetClusterText(ICluster cluster)
        {
            string result;
            var size = cluster.Size / 2;

            if (map.ClusterOptions.EnableBuckets)
            {
                var buckets = map.ClusterOptions.Buckets;
                var bucketIndex = BucketIndexForSize(size);
                result = size.ToString();
            }
            else
                result = size.ToString();

            return result;
        }

        private int BucketIndexForSize(int size)
        {
            uint index = 0;
            var buckets = map.ClusterOptions.Buckets;

            while (index + 1 < buckets.Length && buckets[index + 1] <= size)
                ++index;

            return (int)index;
        }

        // TODO: sửa màu vòng bao cluster
        private LayerDrawable makeClusterBackground()
        {
            mColoredCircleBackground = new ShapeDrawable(new OvalShape());
            ShapeDrawable outline = new ShapeDrawable(new OvalShape());
            outline.Paint.Color = Color.Blue;
            LayerDrawable background = new LayerDrawable(new Drawable[] { outline,
                mColoredCircleBackground });
            int strokeWidth = 3;
            background.SetLayerInset(1, strokeWidth, strokeWidth, strokeWidth,
                    strokeWidth);
            return background;
        }
    }
}
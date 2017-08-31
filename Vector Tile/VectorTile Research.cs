

前端支持矢量瓦片格式：
	1. leaflet： GeoJson
	2. arcgis js4.0 ：满足Mapbox图片存储的格式
	3. openlayers： geojson
	4. flex

ArcGIS：
	1. 切片数据使用pbf文件格式
	2. 请求数据: 
		切片数据（pbf）
		索引文件（json）
		切片信息文件（json)
		地图样式文件（json）
		地图底图文件（json)

MapBox:
	1. File Format: The vector tiles are encoded using Google Protocol Buffers;
	2. File Suffix: .mvt
	3. How to encode attributes that aren’t strings or numbers：
		
		"categories": ["one", "two", "three"]
		TO：
		keys: "categories"
		values: {
			string_value: "[\"one\",\"two\",3]"
		}
		
	4. 投影和范围: Web Mercator是默认的投影方式，Google tile scheme是默认的瓦片编号方式。两者一起完成了与任意范围、任意精度的地理区域的一一对应
	5. 内部结构： 矢量瓦片protobuf编码方案文件
	6. Polygon: 一个外环（顺时针旋转），0个或多个内环（以逆时针旋转），面几何必须不能有内环相交，并且内环必须被包围在内环之中。
	7. 编码： ParameterInteger =(value << 1) ^ (value >> 31)
	8. 解码： value = ((ParameterInteger >> 1) ^ (-(ParameterInteger & 1)))
	9. geometry: based on type to and commands define to encode geometry.
	10. Tile Schema, Geometry encode method.
	
	编码过程：
		1. MapBox 矢量切片规范采用的屏幕坐标是右方向+x，向下+y，左上角为坐标原点，在编码的过程中还会考虑简化要素坐标
		2. 将所有属性key记录，所有的value 记录，分别编号，原数据中相应的 key 和 value 用编号代替
		3. 编码多边形时要注意线行进方向，一个简单的多边形时逆时针的，如果包含环，那么内边界必须是顺时针的，也就是说内边和外边方向是相反的
		4. 根据不同的缩放级别和细节，适当调节简化的程度

Mapnik:
	1. 旧： MVT格式（MVT在内部使用zlib压缩数据，并使用近似的wkb来减少所有几何图形的浮点精度）
		An MVT file starts with 8 bytes. 
			4 bytes "\x89MVT"
			uint32  Length of body
			bytes   zlib-compressed body
		 
		The following body is a zlib-compressed bytestream. When decompressed,
		it starts with four bytes indicating the total feature count.
		 
			uint32  Feature count
			bytes   Stream of feature data
		 
		Each feature has two parts, a raw WKB (well-known binary) representation of
		the geometry in spherical mercator and a JSON blob for feature properties.
		 
			uint32  Length of feature WKB
			bytes   Raw bytes of WKB
			uint32  Length of properties JSON
			bytes   JSON dictionary of feature properties
		 
		By default, encode() approximates the floating point precision of WKB geometry
		to 26 bits for a significant compression improvement and no visible impact on
		rendering at zoom 18 and lower.
			
	2. 新： 采用MapBox矢量瓦片结构
	
Google:
	1. 传输tile数据文件
	2. 文件编码： vnd.google.octet-stream-compressible
	3. 编码说明： 一个二进制私有协议数据流

OpenStreetMap:
	1. https://openmaptiles.org/
	2. OSciM-PBF
	3. TileData_v4.proto

高德地图： json数据
百度地图： png图片
腾讯地图： 压缩的json数据（gzip, deflate）
切片工具：
TileStache
MapServer
GeoServer:
	1. GeoJSON、TopoJSON和.mvt 格式其实都是对数据的重新组织
	2. 一般来说 .mvt 压缩率更高，体积更小
	3. GeoJSON 是比较可读的，比较容易让人看懂
	4. TopoJSON 的可读性比较差，现实中根据实际需求选取矢量切片的格式
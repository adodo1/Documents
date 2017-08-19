最近Mapbox的团队为矢量切片开发了一套开放的说明，这个说明已经成为社区支持的标准。现在已经有十多个公司以及开源项目使用这种标准格式的矢量切片。

ArcGIS：
	1. ArcGIS更倾向于通过采用并改进已有矢量切片说明来支持这种兴起于社区的标准

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
	
Google:


GeoServer:
	1. GeoJSON、TopoJSON和.mvt 格式其实都是对数据的重新组织
	2. 一般来说 .mvt 压缩率更高，体积更小
	3. GeoJSON 是比较可读的，比较容易让人看懂
	4. TopoJSON 的可读性比较差，现实中根据实际需求选取矢量切片的格式
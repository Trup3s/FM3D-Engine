#pragma once
#include "../Dll.h"
#include <Assimp/Importer.hpp>
#include <Assimp/scene.h>
#include <Assimp/postprocess.h>
#include <vector>
#include <string>
#include <map>
#ifdef NO_FM3D
namespace FM3D {
	class Matrix4f;

	template <class T>
	class DynamicRawArray;

	struct MeshPart;
}
#else
#include <Engine.h>
#endif


namespace DesignerLib {

	class ResourceLoader {
	private:
		Assimp::Importer* importer;
		const aiScene* scene;
		std::vector<unsigned int> meshIds;
		FM3D::Matrix4f* meshMatrices;
		FM3D::Matrix4f* globalInverseTransformation;
		std::map<std::string, unsigned int> boneIndex;
		FM3D::DynamicRawArray<FM3D::Matrix4f>* boneOffsetMatrices;
	public:
		~ResourceLoader();
		bool Load(const std::string& path, std::string& mesh, std::vector<std::string>& parts);
		FM3D::MeshPart* GetMeshPart(unsigned int id);
	};

}
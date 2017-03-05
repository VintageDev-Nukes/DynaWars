using UnityEngine;
using System.Collections;

public class RagdollExt {

	public static RagdollBuilder PlayerToRagdoll(RagdollBuilder ragbuild, GameObject player, float mass = 20) {

		ragbuild.root = TransformExt.FindAllChilds(player.transform, "Hips");
		ragbuild.leftHips = TransformExt.FindAllChilds(player.transform, "LeftUpLeg");
		ragbuild.leftKnee = TransformExt.FindAllChilds(player.transform, "LeftLeg");
		ragbuild.leftFoot = TransformExt.FindAllChilds(player.transform, "LeftFoot");
		ragbuild.rightHips = TransformExt.FindAllChilds(player.transform, "RightUpLeg");
		ragbuild.rightKnee = TransformExt.FindAllChilds(player.transform, "RightLeg");
		ragbuild.rightFoot = TransformExt.FindAllChilds(player.transform, "RightFoot");
		ragbuild.leftArm = TransformExt.FindAllChilds(player.transform, "LeftArm");
		ragbuild.leftElbow = TransformExt.FindAllChilds(player.transform, "LeftForeArm");
		ragbuild.rightArm = TransformExt.FindAllChilds(player.transform, "RightArm");
		ragbuild.rightElbow = TransformExt.FindAllChilds(player.transform, "RightForeArm");
		ragbuild.middleSpine = TransformExt.FindAllChilds(player.transform, "Spine");
		ragbuild.head = TransformExt.FindAllChilds(player.transform, "Head");
		ragbuild.totalMass = mass;

		return ragbuild;

	}

}

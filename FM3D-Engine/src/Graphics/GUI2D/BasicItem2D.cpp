#pragma once
#include <Engine.h>


namespace ENGINE_NAME {

	bool BasicItem2D::Click(int keyID) {
		ccresult = CompCoords::PixelToScreenSpace(Inputsystem::GetInstance()->GetLastpos());

		if (Inputsystem::GetInstance()->CheckIfKeyIsPressed(keyID) == true &&
			GetPosition0().y <= ccresult.y &&
			GetPosition0().x <= ccresult.x &&
			GetPosition1().x >= ccresult.x &&
			GetPosition1().y >= ccresult.y) {

			std::cout << "INPUT:: QUD:: /w ID >>" << GetPosition0().z << "<< was pressed" << std::endl;
			Inputsystem::GetInstance()->setKey(keyID, false);
			return true;
		}

		return 0;
	}


	void BasicItem2D::AutoSize() {
		Vector2f pos0px = CompCoords::ScreenSpaceToPixel(Vector2f(GetPosition0().x, GetPosition0().y));
		//SetSize(CompCoords::PixelToScreenSpace(Vector2f(GetTextureWidth() + pos0px.x, GetTextureHeight() + pos0px.y).Subtract(GetPosition0().xy)));
		Vector2f tex((float)GetTextureWidth(), (float)GetTextureHeight());
		CompCoords::ScalePixelToScreenSpace(tex);
		SetPosition1(m_position0.xy + tex);

	}
	///
	///Anchor Basics 
	///
	void BasicItem2D::Stretch() {
		SetPosition0(Vector3f(-1.0f, -1.0f, GetPosition0().z));
		SetPosition1(Vector2f(1.0f, 1.0f));
	}
	void BasicItem2D::Center() {
		VCenter();
		HCenter();
	}
	void BasicItem2D::Left() {
		m_position1.x = -1.0f + (m_position1.x - m_position0.x);
		m_position0.x = -1.0f;
	}
	void BasicItem2D::Right() {
		m_position0.x = 1.0f - (m_position1.x - m_position0.x);
		m_position1.x = 1.0f;
	}
	void BasicItem2D::Top() {
		m_position0.y = 1.0f - (m_position1.y - m_position0.y);
		m_position1.y = 1.0f;
	}
	void BasicItem2D::Bottom() {
		m_position1.y = -1.0f + (m_position1.y - m_position0.y);
		m_position0.y = -1.0f;
	}
	///
	///Vertical Anchor
	///
	void BasicItem2D::VLeft() {
		VCenter();
		Left();
	}
	void BasicItem2D::VRight() {
		VCenter();
		Right();
	}
	void BasicItem2D::VStretch() {
		SetPosition0(Vector3f(GetPosition0().x, -1.0f, GetPosition0().z));
		SetPosition1(Vector2f(GetPosition1().x, 1.0f));
	}
	void BasicItem2D::VCenter() {
		float texturecoord = CompCoords::PixelToScreenSpace(Vector2f(0.0f,(float)GetTextureHeight())).y;
		SetPosition1(Vector2f(GetPosition1().x, -texturecoord));

		SetPosition0(Vector3f(GetPosition0().x, texturecoord, GetPosition0().z));
	}
	///
	///Horizontal Anchor
	///
	void BasicItem2D::HTop() {
		HCenter();
		Top();
	}
	void BasicItem2D::HBottom() {
		HCenter();
		Bottom();
	}
	void BasicItem2D::HStretch() {
		SetPosition0(Vector3f(-1.0f, GetPosition0().y, GetPosition0().z));
		SetPosition1(Vector2f(1.0f, GetPosition1().y));
	}
	void BasicItem2D::HCenter() {
		float texturecoord = CompCoords::PixelToScreenSpace(Vector2f((float)GetTextureWidth(), 0.0f)).x;
		SetPosition1(Vector2f(-texturecoord, GetPosition1().y));
		SetPosition0(Vector3f(+texturecoord, GetPosition0().y, GetPosition0().z));
	}
	///
	///Anchor main method for anchoring
	///
	void BasicItem2D::Anchor(ANCHOR ad) {
		switch (ad)
		{

			//////////////////////////////////////////////////////////////
		case LEFT_CENTER:
			VLeft();
			break;
		case RIGHT_CENTER:
			VRight();
			break;
		case STRETCH_VERTICAL:
			VStretch();
			break;
		case CENTER_VERTICAL:
			VCenter();
			break;

			//////////////////////////////////////////////////////////////
		case TOP_CENTER:
			HTop();
			break;
		case BOTTOM_CENTER:
			HBottom();
			break;
		case STRETCH_HORIZONTAL:
			HStretch();
			break;
		case CENTER_HORIZONTAL:
			HCenter();
			break;

			//////////////////////////////////////////////////////////////
		case STRETCH:
			Stretch();
			break;
		case CENTER:
			Center();
			break;

			//////////////////////////////////////////////////////////////
		case LEFT:
			Left();
			break;
		case RIGHT:
			Right();
			break;
		case TOP:
			Top();
			break;
		case BOTTOM:
			Bottom();
			break;
		default:
			break;
		}
	}
	///
	///Changes the visibility of the picture 
	///
	void BasicItem2D::PicVisibility(float pro) {}
	bool BasicItem2D::ccRectangle(int keyID) {
		ccresult = CompCoords::PixelToScreenSpace(Inputsystem::GetInstance()->GetLastpos());

		if (Inputsystem::GetInstance()->CheckIfKeyIsPressed(keyID) == true &&
			GetPosition0().xy <= ccresult &&
			GetPosition1() >= ccresult) {

			std::cout << "INPUT:: QUD:: /w ID >>" << GetPosition0().z << "<< was pressed" << std::endl;
			Inputsystem::GetInstance()->setKey(keyID, false);
			return true;
		}

		return 0;
	}
	bool BasicItem2D::ccEllipse() { return 0; }
}
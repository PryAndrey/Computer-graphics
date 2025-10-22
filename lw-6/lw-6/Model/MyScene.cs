using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class MyScene
{
    List<BufferData> _bufferedScene = [];

    private readonly MaterialLoader _loader = new MaterialLoader();

    private readonly Model _policeCarModel;
    private readonly Model _supercarModel;
    private readonly Model _pickupCarModel;
    private readonly Model _house1Model;
    private readonly Model _house2Model;
    private readonly Model _house3Model;
    private readonly Model _fenceModel;
    private readonly Model _tractorModel;
    private readonly Model _farmModel;
    private readonly Model _farm2Model;

    public MyScene()
    {
        _policeCarModel = LoadModel("models/police_car.3ds");
        _supercarModel = LoadModel("models/supercar.3ds");
        _pickupCarModel = LoadModel("models/car2.3ds");
        _house1Model = LoadModel("models/house1.3ds");
        _house2Model = LoadModel("models/house2.3ds");
        _house3Model = LoadModel("models/house3.3ds");
        _fenceModel = LoadModel("models/fence.3ds");
        _tractorModel = LoadModel("models/tractor.3ds");
        _farmModel = LoadModel("models/farm1.3ds");
        _farm2Model = LoadModel("models/farm2.3ds");
        // _floorTexture = _loader.GetTextureId("grass.jpg");
        // _roadTexture = _loader.GetTextureId("roadstrip.jpg");
    }
    private Model LoadModel(string path)
    {
        var m = new Model(path);
        return m;
    }

    public void Draw(Renderer renderer)
    {
        
        // renderer.DrawElements(PrimitiveType.TriangleStrip, Centres, Sizes, _bufferedScene);
    }
    
    public Matrix4 GetModelMatrix()
    {
        return Matrix4.Identity;
    }
}
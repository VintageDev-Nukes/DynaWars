Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Linq
Imports System.Drawing.Imaging

Public Class mainApp

    Public Shared gamePath As String

    Public Shared Property loadedItemList() As ItemBase
        Get
            Return m_loadedItemList
        End Get
        Set(value As ItemBase)
            m_loadedItemList = value
            RaiseEvent itemsChanged(Nothing, Nothing)
        End Set
    End Property
    Private Shared m_loadedItemList As ItemBase

    Public Shared Event itemsChanged As EventHandler
    Friend Shared Sub iChanged(e As EventArgs)
        RaiseEvent itemsChanged(Nothing, e)
    End Sub

    Public Sub SortItems()
        Dim base As List(Of Item) = loadedItemList.myBase.OrderBy(Function(x) x.id).ToList()
        loadedItemList = New ItemBase() With {.myBase = base}
    End Sub

End Class

Public Enum ItemList
    Rock
    Stone
    Log
    Branch
    Stick
    Wood
    Leaf
    Sapling
    Dirt
    Grass
    Sand
    Glass
    Snow
    Ice
    Snowball
    Gravel
    Flint
    Lime
    Lime_Dust
    Gypsum
    Mud
    Sulfur
    Sulfur_Dust
    Saltpeter
    Saltpeter_Dust
    Powder
    Resin
    Plastic
    Rubber
    Glue
    Coal
    Charcoal
    Clay
    Brick
    Feather
    Brain
    Zombie_Meat
    Bone
    Fang
    Tooth
    Jaw
    Spine
    Cobweb
    [String]
    Silk
    Wool
    Ink
    Wheat
    Flour
    Egg
    Sugar
    Sweet_Powder
    Cactus
    Bamboo
    Bamboo_Splinter
    Paper
    Cardboard
    Plywood
    M9 = 64
    SawnOff = 65
    MP5 = 66
    Bullets32 = 128
    Gaugue20 = 140
End Enum

Public Enum ItemType
    None
    Gun
    Ammo
    Weapon
    Money
    Equip
    Accesory
    Kits
    Block
    Explosives
End Enum

Public Enum UseType
    None
    Consume
    Potion
    Repair
    Fix
End Enum

<XmlType([Namespace]:="Inventory Items", TypeName:="Inventory")> _
Public Class ItemBase

    Public Sub New()
    End Sub

    <XmlElement("Items")> _
    Public [myBase] As List(Of Item)

End Class

Public Class Item
    Public worldObject As String
    Public id As Integer
    Public itemname As String
    Public DisplayName As String
    Public Description As String
    Public itemtex As String
    Public itemtype As ItemType = DynaWars___Item_Creator.ItemType.None
    Public usable As UseType = UseType.None
    Public weight As Single
    Public droppable As Boolean = True
    Public itemstacksize As Integer = 1
    Public itemstacklimit As Integer = 1
    Public showStack As Boolean = True
    Public TcustomPostion As Vector3
    Public TcustomRotation As Vector3
    Public TcustomScale As Vector3
    Public FcustomPostion As Vector3
    Public FcustomRotation As Vector3
    Public FcustomScale As Vector3
    Public dropObject As String
    Public shopValue As Integer
End Class

<Serializable> _
Public Class Vector3

    Public x As Single
    Public y As Single
    Public z As Single

    Public Sub New(ByVal _x As Single, ByVal _y As Single, ByVal _z As Single)
        Me.x = _x
        Me.y = _y
        Me.z = _z
    End Sub

    Public Sub New()
    End Sub

End Class

Public Class XMLTools

    Public Shared Function Serialize(Of T)(value As T, Optional ByVal indented As Boolean = True) As String
        If value Is Nothing Then
            Throw New Exception("XMLSerializer - The value passed is null!")
            Return ""
        End If
        Try

            Dim xmlserializer As New XmlSerializer(GetType(T))
            Dim serializeXml As String = ""

            Using stringWriter As New StringWriter()

                Using writer As XmlWriter = XmlWriter.Create(stringWriter)
                    xmlserializer.Serialize(writer, value)
                    serializeXml = stringWriter.ToString()
                End Using

                If indented Then
                    serializeXml = XMLBeautify(serializeXml)
                End If

            End Using

            Return serializeXml
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        End Try
    End Function

    Public Shared Function Deserialize(Of T)(value As String) As T

        Try
            Dim returnvalue As New Object()
            Dim xmlserializer As New XmlSerializer(GetType(T))
            Dim reader As TextReader = New StringReader(value)

            returnvalue = xmlserializer.Deserialize(reader)

            reader.Close()
            Return DirectCast(returnvalue, T)
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Sub SerializeToFile(Of T)(value As T, filePath As String, Optional ByVal indented As Boolean = True)
        If value Is Nothing Then
            Throw New Exception("XMLSerializer - The value passed is null!")
        End If
        Try
            Dim xmlserializer As New XmlSerializer(GetType(T))
            Using fileWriter As StreamWriter = New StreamWriter(filePath)
                If indented Then
                    Using stringWriter As New StringWriter()
                        Using writer As XmlWriter = XmlWriter.Create(stringWriter)
                            xmlserializer.Serialize(writer, value)
                            fileWriter.WriteLine(XMLBeautify(stringWriter.ToString()))
                        End Using
                    End Using
                Else
                    Using writer As XmlWriter = XmlWriter.Create(fileWriter)
                        xmlserializer.Serialize(writer, value)
                    End Using
                End If
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function DeserializeFromFile(Of T)(filePath As String) As T

        Try
            Dim returnvalue As New Object()
            Dim xmlserializer As New XmlSerializer(GetType(T))
            Using reader As TextReader = New StreamReader(filePath)
                returnvalue = xmlserializer.Deserialize(reader)
            End Using
            Return DirectCast(returnvalue, T)
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Function XMLBeautify(ByVal XMLText As String,
                                       Optional ByVal IndentChars As String = Nothing,
                                       Optional ByVal IndentOnAttributes As Boolean = False,
                                       Optional ByVal TextEncoding As System.Text.Encoding = Nothing) As String

        If String.IsNullOrEmpty(XMLText) Then
            Throw New ArgumentNullException(XMLText)
        End If

        Dim sb As New System.Text.StringBuilder
        Dim doc As New XmlDocument()
        Dim settings As New XmlWriterSettings

        With settings
            .Indent = True
            .CheckCharacters = True
            .OmitXmlDeclaration = False
            .ConformanceLevel = ConformanceLevel.Auto
            .NamespaceHandling = NamespaceHandling.Default
            .NewLineHandling = NewLineHandling.Replace
            .NewLineChars = ControlChars.NewLine
            .NewLineOnAttributes = IndentOnAttributes
            .IndentChars = If(IndentChars IsNot Nothing, IndentChars, ControlChars.Tab)
            .Encoding = If(TextEncoding IsNot Nothing, TextEncoding, System.Text.Encoding.Default)
        End With

        Using writer As XmlWriter = XmlWriter.Create(sb, settings)
            doc.LoadXml(XMLText)
            doc.WriteContentTo(writer)
            writer.Flush()
            Return sb.ToString
        End Using

    End Function
    Public Shared Function XMLBeautify(XMLFile As IO.FileInfo,
                                       Optional ByVal IndentChars As String = Nothing,
                                       Optional ByVal IndentOnAttributes As Boolean = False,
                                       Optional ByVal TextEncoding As System.Text.Encoding = Nothing) As String

        Return XMLBeautify(IO.File.ReadAllText(XMLFile.FullName, TextEncoding), IndentChars, IndentOnAttributes, TextEncoding)

    End Function

End Class

Public Class ImageTools

    Public Shared Sub ColorToTransparent(ByVal imagePath As String, ByVal sColor As Color, ByVal path As String)
        Dim newImage As Image = Image.FromFile(imagePath)
        ColorToTransparent(newImage, sColor, path)
    End Sub

    Public Shared Sub ColorToTransparent(ByVal myImage As Image, ByVal sColor As Color, ByVal path As String)
        Using b As New Bitmap(myImage.Width, myImage.Height)
            b.SetResolution(myImage.HorizontalResolution, myImage.VerticalResolution)

            Using g As Graphics = Graphics.FromImage(b)
                g.Clear(sColor)
                g.DrawImageUnscaled(myImage, 0, 0)

                b.Save(path)
            End Using
        End Using
    End Sub

    Public Shared Function Tint(ByVal bmpSource As Bitmap, ByVal clrScaleColor As Color, ByVal contrastFactor As Single, ByVal opacity As Single, ByVal sngScaleDepth As Single) As Bitmap

        Dim bmpTemp As New Bitmap(bmpSource.Width, bmpSource.Height) 'Create Temporary Bitmap To Work With

        Dim iaImageProps As New ImageAttributes 'Contains information about how bitmap and metafile colors are manipulated during rendering. 

        Dim cmNewColors As ColorMatrix 'Defines a 5 x 5 matrix that contains the coordinates for the RGBAW space

        cmNewColors = New ColorMatrix(New Single()() _
            {New Single() {contrastFactor, 0, 0, 0, 0}, _
             New Single() {0, contrastFactor, 0, 0, 0}, _
             New Single() {0, 0, contrastFactor, 0, 0}, _
             New Single() {0, 0, 0, 1, 0}, _
             New Single() {clrScaleColor.R / 255 * sngScaleDepth, clrScaleColor.G / 255 * sngScaleDepth, clrScaleColor.B / 255 * sngScaleDepth, 0, 1}})

        cmNewColors.Matrix33 = opacity

        iaImageProps.SetColorMatrix(cmNewColors) 'Apply Matrix

        Dim grpGraphics As Graphics = Graphics.FromImage(bmpTemp) 'Create Graphics Object and Draw Bitmap Onto Graphics Object

        grpGraphics.DrawImage(bmpSource, New Rectangle(0, 0, bmpSource.Width, bmpSource.Height), 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, iaImageProps)

        Return bmpTemp

    End Function

End Class
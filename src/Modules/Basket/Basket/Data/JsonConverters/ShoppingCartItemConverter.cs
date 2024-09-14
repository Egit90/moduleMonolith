using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Basket.Models;

namespace Basket.Data.JsonConverters;

public sealed class ShoppingCartItemConverter : JsonConverter<ShoppingCartItem>
{
    public override ShoppingCartItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var id = rootElement.GetProperty("id").GetGuid();
        var ShoppingCartId = rootElement.GetProperty("shoppingCartId").GetGuid();
        var productId = rootElement.GetProperty("productId").GetGuid();
        var quantity = rootElement.GetProperty("quantity").GetInt32();
        var color = rootElement.GetProperty("color").GetString()!;
        var price = rootElement.GetProperty("price").GetDecimal();
        var productName = rootElement.GetProperty("productName").GetString()!;

        return new ShoppingCartItem(id, ShoppingCartId, productId, quantity, color, price, productName);
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCartItem value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("shoppingCartId", value.ShoppingCartId.ToString());
        writer.WriteString("productId", value.ProductId.ToString());
        writer.WriteNumber("quantity", value.Quantity);
        writer.WriteString("color", value.Color);
        writer.WriteNumber("price", value.Price);
        writer.WriteString("productName", value.ProductName);

        writer.WriteEndObject();
    }
}
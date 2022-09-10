import { useParams } from 'react-router-dom';
import { ProductsType } from '../../productType';

type Props = {
  datas?: ProductsType[] | undefined;
};


export default function Detail() {
  const params = useParams();
  const id = params.id;

  const data = [
    {
      "id": 1,
      "title": "Microsoft Surface Laptop 4",
      "description": "Style and speed. Stand out on HD video calls backed by Studio Mics. Capture ideas on the vibrant touchscreen. Do it all with a perfect balance of sleek design, speed, immersive audio, and significantly longer battery life than before.",
      "price": 1900,
      "customerprice": 14900,
      "boughtprice": 14900,
      "stock": 18,
      "brand": "Microsoft",
      "category": "Laptops",
      "imageurl": "https://fdn.gsmarena.com/imgroot/news/21/09/surface-laptops/-1200/gsmarena_001.jpg",
      "coupon": 50
    },
    {
      "id": 2,
      "title": "iPhone 13 Pro",
      "description": "The iPhone 13 display has rounded corners that follow a beautiful curved design, and these corners are within a standard rectangle. When measured as a standard rectangular shape, the screen is 6.06 inches diagonally",
      "price": 13450,
      "stock": 25,
      "brand": "Apple",
      "category": "Phones",
      "imageurl": "https://fdn2.gsmarena.com/vv/pics/apple/apple-iphone-13-01.jpg",
      "coupon": 45
    },
    {
      "id": 3,
      "title": "JBL Tune 510BT Headphone",
      "description": "The Tune 510BT wireless headphones feature renowned JBL Pure Bass sound, which can be found in the most famous venues all around the world With Wireless Bluetooth 5.0 Streaming, you can stream wirelessly from your device and even switch between two devices so that you don't miss a call",
      "price": 3500,
      "stock": 15,
      "brand": "JBL",
      "category": "Headphones",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/61yjoRgptdL._AC_SX425_.jpg",

      "coupon": 10
    },
    {
      "id": 4,
      "title": "Apple AirPods Pro",
      "description": "Apple AirPods Pro Wireless Earbuds with MagSafe Charging Case. Active Noise Cancelling, Transparency Mode, Spatial Audio, Customizable Fit, Sweat and Water Resistant. Bluetooth Headphones for iPhone",
      "price": 2400,
      "stock": 20,
      "brand": "Apple",
      "category": "Headphones",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/71bhWgQK-cL._AC_SX466_.jpg",
      "coupon": 10
    },

    {
      "id": 5,
      "title": "Amazon Basics 3-Button Mouse",
      "description": "Computer mouse for easily navigating a computer interface click, scroll, and more Includes a USB-connected wired black mouse with 3 buttons for effortless fingertip control High-definition (1000 dpi) optical tracking nsures responsive cursor control for precise tracking and easy text selection",
      "price": 300,
      "stock": 35,
      "brand": "Amazonbasics",
      "category": "Mouse&Keyboard",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/61i0CV-tKpL._AC_SX425_.jpg",
      "coupon": 5
    },
    {
      "id": 6,
      "title": "Lovaky MK98 Wireless Keyboard",
      "description": "Computer mouse for easily navigating a computer interface click, scroll, and more Includes a USB-connected wired black mouse with 3 buttons or effortless fingertip control. High-definition (1000 dpi) optical tracking ensures responsive cursor control for precise tracking and easy text selection",
      "price": 1200,
      "stock": 28,
      "brand": "Lovaky",
      "category": "Mouse&Keyboard",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/61fie1u7QAL._AC_SX679_.jpg",
      "coupon": 10
    },

    {
      "id": 7,
      "title": "Acer Nitro 5 AN515-55-53E5 Gaming Laptop",
      "description": "Dominate the Game: With the 10th Gen Intel Core i5-10300H processor, your Nitro 5 is packed with incredible power for all your games. Internal SpecificationsHard Drive Bay Available) ",
      "price": 18000,
      "stock": 28,
      "brand": "Acer",
      "category": "Laptops",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/81bc8mA3nKL._AC_SX425_.jpg",
      "coupon": 60
    },
    {
      "id": 8,
      "title": "Acer Full Gaming Laptop model 22",
      "description": "Products with electrical plugs are designed for use in the US. Outlets and voltage differ internationally and this product may require an adapter or converter for use in your destination. Please check compatibility before purchasing ",
      "price": 12000,
      "stock": 8,
      "brand": "Acer",
      "category": "Laptops",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/71RK6+rx-xL._AC_SX679_.jpg",
      "coupon": 50
    },
    {
      "id": 9,
      "title": "GIGABYTE G5 KD - 15.6 FHD",
      "description": "NVIDIA GeForce RTX 3060 Laptop GPU GDDR6 6GB , Boost Clock 1485 Mhz & Max Graphics Power of 75W , All-zone of Single Colored Backlit Keyboard with 15 Colors LED Setting",
      "price": 13000,
      "stock": 8,
      "brand": "Gigabyte",
      "category": "Laptops",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/71RK6+rx-xL._AC_SX679_.jpg",
      "coupon": 50
    },
    {
      "id": 10,
      "title": "HP DeskJet 2755 Wireless",
      "description": "This pre-owned product has been professionally inspected, tested and cleaned by Amazon qualified vendors. This product is in Excellent condition. The screen and body show no signs of cosmetic damage visible from 12 inches away. ",
      "price": 3600,
      "stock": 15,
      "brand": "HP",
      "category": "Printer",
      "imageurl": "https://m.media-amazon.com/images/W/WEBP_402378-T1/images/I/71w39ba633L._AC_SX425_.jpg",
      "coupon": 20
    }
  ];
  const show = data.map((row)=>{
    return row.id.toString()==id ?  <>
    <div>
        <img src={row.imageurl} width="60%" />{' '}
      </div>
      <div style={{ width: '60%' }}>
        <h1>{row.title}</h1>
        <h5>{row.description}</h5>
        <strong style={{ color: 'green', fontSize: 22 }}>
          Price: {row.price}
        </strong>
      </div>
    </>: <div>Page Not Found</div>

    })

  return (<>{show}</>)
}

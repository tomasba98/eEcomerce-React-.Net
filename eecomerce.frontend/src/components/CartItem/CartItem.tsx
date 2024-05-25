import React, { useContext } from "react";
import { IoMdAdd, IoMdClose, IoMdRemove } from "react-icons/io";
import { CartContext } from "../../context/CartContext";
import { Link } from "react-router-dom";
import { Product } from "../../types/Product";

interface CartItemProps {
  product: Product & { amount: number };
}

const CartItem: React.FC<CartItemProps> = ({ product }) => {
  const { removeFromCart, increaseAmount, decreaseAmount } = useContext(CartContext);


  return (
    <div className="flex gap-x-4 py-2 lg:px-6 border-b border-gray-200 w-full font-light text-gray-500">
      <div className="w-full min-h-[150px] flex items-center gap-x-4">
        <Link to={`/product/${product.id}`}>
          <a>
            <img className="max-w-[80px]" src={product.image}/>
          </a>
        </Link>
        <div className="w-full flex flex-col">
          <div className="flex justify-between mb-2">
            <Link to={`/product/${product.id}`}>
              <a className="text-sm uppercase font-medium max-w-[240px] text-primary hover:underline">{product.name}</a>
            </Link>
            <div onClick={() => removeFromCart(product.id)} className="text-xl cursor-pointer">
              <IoMdClose className="text-gray-500 hover:text-red-500 transition" />
            </div>
          </div>
          <div className="flex gap-x-2 h-[36px] text-sm">
            <div className="flex flex-1 max-w-[100px] items-center h-full border text-primary font-medium">
              <div onClick={() => decreaseAmount(product.id)} className="h-full flex-1 flex justify-center items-center cursor-pointer">
                <IoMdRemove />
              </div>
              <div className="h-full flex justify-center items-center px-2">{product.amount}</div>
              <div onClick={() => increaseAmount(product.id)} className="h-full flex flex-1 justify-center items-center cursor-pointer">
                <IoMdAdd />
              </div>
            </div>
            <div className="flex flex-1 justify-around items-center">$ {product.price}</div>
            <div className="flex flex-1 justify-end items-center text-primary font-medium">{`$ ${(product.price * product.amount).toFixed(2)}`}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CartItem;

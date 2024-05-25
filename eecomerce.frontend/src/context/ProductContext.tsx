import React, { createContext, useState, useEffect } from "react";
import { Product } from "../types/Product";

export const ProductContext = createContext<{ products: Product[] }>({ products: [] });

const ProductProvider = ({ children }: { children: React.ReactNode }) => {
  // products state
  const [products, setProducts] = useState<Product[]>([]);

  // fetch products
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await fetch("https://fakestoreapi.com/products");
        if (!response.ok) {
          throw new Error("Failed to fetch products");
        }
        const data = await response.json();
        setProducts(data);
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    };
    fetchProducts();
  }, []);

  return (
    <ProductContext.Provider value={{ products }}>
      {children}
    </ProductContext.Provider>
  );
};

export default ProductProvider;

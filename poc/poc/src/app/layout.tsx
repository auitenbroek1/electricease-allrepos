import {
  ClerkProvider,
  SignInButton,
  SignedIn,
  SignedOut,
  UserButton,
} from "@clerk/nextjs";

import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "POC",
  description: "",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const enabled = true;
  return (
    <ClerkProvider>
      <html lang="en">
        <body
          className={`${geistSans.variable} ${geistMono.variable} antialiased`}
        >
          {enabled && (
            <>
              <SignedOut>
                <div className="absolute top-0 right-0 p-8">
                  <SignInButton />
                </div>
              </SignedOut>
              <SignedIn>
                <div className="absolute top-0 right-0 p-8">
                  <UserButton />
                </div>
                {children}
              </SignedIn>
            </>
          )}
        </body>
      </html>
    </ClerkProvider>
  );
}

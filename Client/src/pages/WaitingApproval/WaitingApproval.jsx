import React from 'react';
import { Clock, Mail, ArrowRight } from 'lucide-react';
import { Helmet } from "react-helmet-async";
import { Link } from 'react-router-dom';
import '../Login/glow.css'

const WaitingApproval = () => {
  return (
    <>
      <Helmet>
        <title>Waiting Approval | Time Fourthe</title>
        <link rel="icon" type="image/png" href="/home-icon.png" />
      </Helmet>
      <div className="min-h-screen bg-black flex items-center justify-center p-4 relative overflow-hidden">
        <div className="absolute inset-0 bg-black opacity-50 blur-2xl" />
        <div className="max-w-[75vw] w-full space-y-8 bg-white/5 p-8 rounded-2xl backdrop-blur-md border border-white/10 shadow-md relative z-10 transition-all duration-300 glow-border hover:shadow-white/20">
          <div className="text-center">
            <div className="flex justify-center mb-6 animate-pulse">
              <div className="p-4 rounded-full bg-white/10 border border-white/10 shadow-md">
                <Clock className="h-12 w-12 text-white" />
              </div>
            </div>
            <h2 className="text-3xl font-bold text-white mb-4 tracking-wide drop-shadow-md">
              Waiting for Approval
            </h2>
            <p className="text-gray-400 text-lg mb-8  mx-auto leading-relaxed">
             <div> Your account is pending approval from Time Fourthe administrators</div> <div> Please check your email for further instructions</div>
            </p>
            
            <div className="flex items-center justify-center space-x-2 text-white/70 mb-8 animate-fadeIn">
              <Mail className="h-5 w-5 text-gray-400" />
              <span className="text-gray-300">Check your email inbox</span>
            </div>

            <div className="space-y-4">
              <p className="text-gray-500 text-sm">
                Once approved, you'll receive an email with login instructions.
              </p>
              <Link 
                to="/login"
                className="inline-flex items-center text-white hover:text-gray-300 transition-all duration-300 group"
              >
                Return to login
                <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform duration-300" />
              </Link>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default WaitingApproval;

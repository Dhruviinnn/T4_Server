import React from 'react'
import { timetableImages } from '../../public/images';

const Images = () => {
    const positions = [
        { top: '5%', left: '60%', transform: 'rotate(-10deg)' },
        { top: '25%', left: '50%', transform: 'rotate(15deg)' },
        { top: '25%', left: '90%', transform: 'rotate(-5deg)' },
        { top: '35%', left: '70%', transform: 'rotate(20deg)' },
        { top: '50%', left: '75%', transform: 'rotate(-15deg)' },
        { top: '10%', left: '55%', transform: 'rotate(5deg)' },
        { top: '70%', left: '95%', transform: 'rotate(-8deg)' },
        { top: '80%', left: '80%', transform: 'rotate(12deg)' },
      ];
    
    return (
        <div className="hidden lg:block w-1/2 bg-gradient-to-br from-white/5 via-white/3 to-transparent relative overflow-hidden">
            <div className="absolute inset-0 bg-black/40" />
            {timetableImages.map((image, index) => (
                <div
                    key={index}
                    className="absolute w-[500px] h-[300px] rounded-xl overflow-hidden"
                    style={{
                        top:positions[index],
                        left: positions[index],
                        transform: image.transform,
                        transition: 'all 0.5s ease-out',
                    }}
                >
                    <div
                        className="w-full h-full bg-cover bg-center opacity-10 hover:opacity-20 transition-opacity duration-300"
                        style={{ backgroundImage: `url(${image.url})` }}
                    />
                </div>
            ))}
        </div>
    )
}

export default Images